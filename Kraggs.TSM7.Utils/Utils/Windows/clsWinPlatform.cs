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

using System.IO;
using System.Diagnostics;
using System.Security.AccessControl;

namespace Kraggs.TSM7.Utils.Windows
{
    /// <summary>
    /// TSM Platform code for Windows.
    /// </summary>
    internal class clsWinPlatform : absTSMPlatform
    {
        internal string sBAClientPath;
        internal string sDsmcBinary;

        internal string sDsmAdmcBinary;
        internal Version sDsmAdmcVersion;

        public clsWinPlatform()
        {
            var regBAClient = clsTSMRegistry.GetProductByName("TSM Backup Archive Client");
            if(regBAClient != null)
            {
                if (Directory.Exists(regBAClient.Path))
                    sBAClientPath = regBAClient.Path;

                var testpath = Path.Combine(sBAClientPath, "dsmc.exe");
                if (File.Exists(testpath))
                    sDsmcBinary = testpath;
            }

            var regDsmAdmc = clsTSMRegistry.GetProductByName("TSM Administrative Client");
            if(regDsmAdmc != null)
            {
                var testPath = Path.Combine(regDsmAdmc.Path, "dsmadmc.exe");

                if (File.Exists(testPath))
                {
                    sDsmAdmcBinary = testPath;
                    sDsmAdmcVersion = regDsmAdmc.PtfLevel;
                }
            }
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
        
        [DebuggerNonUserCode()]
        public override Version DsmAdmcVersion
        {
            get { return sDsmAdmcVersion; }
        }

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
                return false; // in case someone chooses continue for some reasen.
            }


            /*
                THIS CODE IS REMOVED.
                Alternative is to create a sofisticated set of functions for
                detecting if UAC is enabled or not, if user is an Admin or Not,
                if this process has write access to dsmerror.log or not.

                Merely checking if this process has write access to dsmerrorlog 
                is not sufficient since we get an exceptino when another dsmcad
                or other tsm processes uses the dsmerror.log.

                So for now we are going to check for this error in the return result
                rather than before, and maybe later implement such functions as
                mentioned above when tested correctly...
            */
            //var dsmerrorlog = Path.Combine(this.BAClientPath, "dsmerror.log");

            //// use try/catch here to apply a more detailed exception instead of a generic not access exception.  
            //try
            //{
            //    //var f = File.OpenWrite(dsmerrorlog);
            //    //var fi = new FileInfo(dsmerrorlog);

            //    //var ac = fi.GetAccessControl(AccessControlSections.Access);

            //    //ac.

            //    //var f = File.OpenRead(dsmerrorlog);
            //    //f.Close();
            //}
            ////catch (UnauthorizedAccessException uae)
            //catch (UnauthorizedAccessException uae)
            //{
            //    var message = string.Format(
            //        "DsmAdmc requires write access to file '{0}'. Either run as elevated or have an admin grant write access to that file to ensure correct dsmadmc handling.", dsmerrorlog);

            //    throw new UnauthorizedAccessException(message, uae);
            //    return false; // in case someone chooses continue for some reasen.
            //}

            return true;
        }
    }
}
