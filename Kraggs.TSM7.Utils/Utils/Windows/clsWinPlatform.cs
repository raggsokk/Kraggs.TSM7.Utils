﻿#region License
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

using System.IO;
using System.Diagnostics;
//using System.Security.AccessControl;

namespace Kraggs.TSM7.Utils.Windows
{
    /// <summary>
    /// TSM Platform code for Windows.
    /// </summary>
    internal class clsWinPlatform : absTSMPlatform
    {
        //internal static readonly string DEFAULT_PATH = @"/opt/tivoli/tsm/client/ba/bin";
        internal static readonly string DEFAULT_PATH = @"C:\Program Files\Tivoli\TSM\BAClient";

        internal string sBAClientPath;
        internal string sDsmcBinary;

        internal string sDsmAdmcBinary;
        //internal Version sDsmAdmcVersion;

        public clsWinPlatform()
        {
            if (Directory.Exists(DEFAULT_PATH))
                this.sBAClientPath = DEFAULT_PATH;

            var testpath = Path.Combine(DEFAULT_PATH, "dsmc.exe");
            if (File.Exists(testpath))
                sDsmcBinary = testpath;

            testpath = Path.Combine(DEFAULT_PATH, "dsmadmc.exe");
            if (File.Exists(testpath))
                sDsmAdmcBinary = testpath;

            //var regBAClient = clsTSMRegistry.GetProductByName("TSM Backup Archive Client");
            //if(regBAClient != null)
            //{
            //    if (Directory.Exists(regBAClient.Path))
            //        sBAClientPath = regBAClient.Path;

            //    var testpath = Path.Combine(sBAClientPath, "dsmc.exe");
            //    if (File.Exists(testpath))
            //        sDsmcBinary = testpath;
            //}

            //var regDsmAdmc = clsTSMRegistry.GetProductByName("TSM Administrative Client");
            //if(regDsmAdmc != null)
            //{
            //    var testPath = Path.Combine(regDsmAdmc.Path, "dsmadmc.exe");

            //    if (File.Exists(testPath))
            //    {
            //        sDsmAdmcBinary = testPath;
            //        sDsmAdmcVersion = regDsmAdmc.PtfLevel;
            //    }
            //}
        }

        [DebuggerNonUserCode()]
        public override string DsmcBinary
        {
            get { return sDsmcBinary; }
        }

        [DebuggerNonUserCode()]
        public override string DsmAdmcBinary
        {
            get { return sDsmAdmcBinary; }
        }

        [DebuggerNonUserCode()]
        public override string BAClientPath
        {
            get { return sBAClientPath; }
        }
        
        //[DebuggerNonUserCode()]
        //public override Version DsmAdmcVersion
        //{
        //    get { return sDsmAdmcVersion; }
        //}

		/// <summary>
		/// Returns on linux/mac the full path to the default dsm.sys file aka [BAClientPath]\dsm.sys
		/// On Windows it will throw an exception.
		/// DOES NOT CHECK IF FILE EXISTS.
		/// </summary>
		/// <value>The dsm sys.</value>
		[DebuggerNonUserCode()]
		public override string DsmSys {
			get {
				//return base.DsmSys;
				throw new InvalidOperationException("Dsm.Sys is not used on this platform");
			}
		}

        /// <summary>
        /// Validates that the current running environment is setup correctly.
        /// Specifically it checks for existance of dsm.opt and write access to dsmerror.log.
        /// </summary>
        /// <returns>The platform.</returns>
        public override bool ValidatePlatform()
        {
            //TODO: Why have return here at all? Why not just throw exceptions?

            var defaultOpt = Path.Combine(this.BAClientPath, "dsm.opt");

            if (!File.Exists(defaultOpt))
            {
                var message = string.Format(
                    "DsmAdmc requires that file '{0}' exists, even if empty, for correctly running.", defaultOpt);
                throw new FileNotFoundException(message, defaultOpt);
                //return false; // in case someone chooses continue for some reasen.
            }

            // removed dsmerror.log writeaccess checks since not needed anymore.

            // Add additionally environment checks here.

            return true;
        }
    }
}
