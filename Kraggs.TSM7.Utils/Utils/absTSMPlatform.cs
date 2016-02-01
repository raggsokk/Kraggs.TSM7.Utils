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

namespace Kraggs.TSM7.Utils
{
    /// <summary>
    /// Platform specific code should be hidden behind this api.
    /// </summary>
    internal abstract class absTSMPlatform
    {
        /// <summary>
        /// Returns the full path to the default BAClient install path on current platform.
        /// </summary>
        public abstract string BAClientPath { get; }

        /// <summary>
        /// Returns the full path to the default dsmc binary on current platform.
        /// </summary>
        public abstract string DsmcBinary { get; }
        /// <summary>
        /// Returns the full path to the default dsmadmc binary on current platform.
        /// </summary>
        public abstract string DsmAdmcBinary { get; }

        /// <summary>
        /// Returns the version of the dsmadmc binary.
        /// Or null if not able to.
        /// </summary>
        public virtual Version DsmAdmcVersion
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the full path to the default dsm.opt file aka [BAClientPath]\dsm.opt
        /// DOES NOT CHECK IF FILE EXISTS.
        /// </summary>
        [DebuggerNonUserCode()]
        public virtual string DsmOpt 
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(BAClientPath))
                    return Path.Combine(BAClientPath, "dsm.opt");
                else
                    return null;
            }
        }
        
        /// <summary>
        /// Returns on linux/mac the full path to the default dsm.sys file aka [BAClientPath]\dsm.sys
        /// On Windows it will throw an exception.
        /// DOES NOT CHECK IF FILE EXISTS.
        /// </summary>
		[DebuggerNonUserCode()]
        public virtual string DsmSys 
        {
            get
            {
                //throw new InvalidOperationException("Dsm.Sys is not used on this platform");
				// for now new default is get this.
				return Path.Combine(BAClientPath, "dsm.sys");
            }
        }

        /// <summary>
        /// Returns a user writable directory for use when redirecting dsmerror.log and when tmp outputting data to file.
        /// </summary>
        /// <returns></returns>
        public virtual string GetUserWritableDirectory()
        {
            return Path.GetTempPath();
        }

        /// <summary>
        /// Validates that the current runnning platform is setup for running dsmadmc.
        /// (For Windows: existanse of default dsm.opt and write access to dsmerror.log.
        /// </summary>
        /// <returns>The platform.</returns>
        public virtual bool ValidatePlatform()
        {
            return true;
        }



        //// only valid on windows.
        //public abstract string DsmcUtilBinary { get; } 
        //public abstract string TDPExchangePath { get; }
        //public abstract string TDPSQLPath { get; }

        //// on linux also but still undecided since this might be x32/x64
        //public abstract string TDPOracle32Path { get; protected set; }
        //public abstract string TDPOracle64Path { get; protected set; }

        //// undecided about these.
        //public abstract string API64Path { get; }
        //public abstract string API32Path { get; }

    }
}
