#region License
/*
    TSM 7.1 Utility library.
    Copyright (C) 2014 Jarle Hansen

    This library is free software; you can redistribute it and/or
    modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public
    License along with this library; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/
#endregion

using System;
using System.Collections.Generic;
using System.Text;

using System.Diagnostics;

using Microsoft.Win32;

namespace Kraggs.TSM7.Utils.Windows
{
    internal static class clsTSMRegistry
    {
        internal const string REGISTRY_ADSM_CURRENTVERSION = @"SOFTWARE\IBM\ADSM\CurrentVersion";

        private static Dictionary<string, clsTSMProduct> sTSMProducts;

        static clsTSMRegistry()
        {

            Initialize(REGISTRY_ADSM_CURRENTVERSION);
        }

        /// <summary>
        /// Initializes product catalog from registry into dictionary.
        /// This way we can run nunit tests with test data.
        /// </summary>
        /// <param name="RegKey"></param>
        internal static void Initialize(string RegKey = null)
        {
            if (RegKey == null)
                RegKey = REGISTRY_ADSM_CURRENTVERSION;

            if (sTSMProducts == null)
                sTSMProducts = new Dictionary<string, clsTSMProduct>();
            else
                sTSMProducts.Clear();

            if(!Environment.Is64BitOperatingSystem)
            {
                // if not 64 bit os, skip separate 32 and 64 check.
                var adsmcur = Registry.LocalMachine.OpenSubKey(RegKey);

                EnumerateAdsmCur(adsmcur);
            }
            else
            {
                var hklm32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
                var adsmcur32 = hklm32.OpenSubKey(RegKey);
                EnumerateAdsmCur(adsmcur32);

                var hklm64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                var adsmcur64 = hklm64.OpenSubKey(RegKey);
                EnumerateAdsmCur(adsmcur64);
            }
        }

        private static bool EnumerateAdsmCur(RegistryKey AdsmCur)
        {
            try
            {
                var components = AdsmCur.OpenSubKey("ComponentsMsi");

                foreach(var keyname in components.GetValueNames())
                {
                    clsTSMProduct prod;
                    if(keyname == "TSM Backup Archive Client")
                        prod = new clsTSMBAClientProduct();
                    else
                        prod = new clsTSMProduct();

                    // read in product entry
                    prod.Component = keyname;
                    prod.RegSubKey = components.GetValue(keyname) as string;
                    
                    // read in separate product info.
                    var prodkey = AdsmCur.OpenSubKey(prod.RegSubKey);
                    prod.Path = prodkey.GetValue("Path") as string;
                    prod.PtfLevel = new Version(prodkey.GetValue("PtfLevel") as string);


                    // BackupClient reg extended info retrival.
                    if(prod.RegSubKey == "BackupClient")
                    {
                        var baprod = prod as clsTSMBAClientProduct;
                        Debug.Assert(baprod != null);

                        var adsmsyspath = prodkey.GetValue("DefaultVssStagingDir", string.Empty) as string;
                        if (adsmsyspath != string.Empty)
                            baprod.DefaultVssStagingDir = adsmsyspath;

                        // Smaller codebase means less chance of bugs. chopping this out until needed.
                        //var keyLanguages = prodkey.OpenSubKey("Languages");
                        //if (keyLanguages != null)
                        //{
                        //    foreach(var lang in keyLanguages.GetValueNames())
                        //    {
                        //        var langcodeO = keyLanguages.GetValue(lang);

                        //    }
                        //}
                    }                    
                    

                    sTSMProducts.Add(prod.Component, prod);
                    prodkey.Dispose();
                }

                components.Dispose();

                return true;
            }
            catch
            { 
                return false;
            }
        }

        public static clsTSMProduct GetProductByName(string ProductName)
        {
            clsTSMProduct product;

            if (sTSMProducts.TryGetValue(ProductName, out product))
                return product;
            else
                return null;
        }

    }
}
