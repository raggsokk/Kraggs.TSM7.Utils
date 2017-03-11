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

// doesnt work on .net framwork.
//using System.Runtime.InteropServices;

namespace Kraggs.TSM7.Utils
{
    internal static class clsTSMPlatform
    {
        internal static readonly absTSMPlatform sThisPlatform;

		public static readonly bool IsLinux;
		public static readonly bool IsMacOSX;
		public static readonly bool IsWindows;

        static clsTSMPlatform()
        {
            //TODO: Handle OS Detection better than this.

            var dirWindows = Environment.GetEnvironmentVariable("windir");

            if(!string.IsNullOrWhiteSpace(dirWindows) && dirWindows.Contains("\\") && Directory.Exists(dirWindows))
            {
                IsWindows = true;
                sThisPlatform = new Windows.clsWinPlatform();
            }            
            else if(File.Exists(@"/proc/sys/kernel/ostype"))
            {
                var osType = File.ReadAllText(@"/proc/sys/kernel/ostype");

                if (osType.ToUpperInvariant().Contains("LINUX"))
                {
                    IsLinux = true;
                    sThisPlatform = new Linux.clsLinuxPlatform();
                }
                else
                    throw new PlatformNotSupportedException($"Platform '{osType}' is currently not supported");
            }
            else if(File.Exists(@"/System/Library/CoreServices/SystemVersion.plist"))
            {
                IsMacOSX = true;
                sThisPlatform = new MacOSX.clsMacOSXPlatform();
            }
            else
            {
                throw new PlatformNotSupportedException("Failed to detect OS Platform we are running on.");
            }



            //if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))            
            //{
            //    sThisPlatform = new Windows.clsWinPlatform();
            //    IsWindows = true;
            //}
            //else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            //{
            //    sThisPlatform = new Linux.clsLinuxPlatform();
            //    IsLinux = true;
            //}
            //else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            //{
            //    sThisPlatform = new MacOSX.clsMacOSXPlatform();
            //    IsMacOSX = true;
            //}
            //else
            //{
            //    throw new PlatformNotSupportedException();
            //}

            //var r = new RuntimeInformation();

            //var r = System.Runtime.InteropServices.RuntimeInformation.

			//var osplatform = Environment.OSVersion.Platform;

			//if(osplatform == PlatformID.MacOSX) {
			//	IsMacOSX = true;
			//	sThisPlatform = new MacOSX.clsMacOSXPlatform();
			//} else if(osplatform == PlatformID.Unix) {

			//	// this can still be maxosx unfortunately.

			//	var flagRunningMac = MacOSX.clsMacOSXPlatform.GetIsRunningMac();

			//	if(flagRunningMac) {
			//		IsMacOSX = true;
			//		sThisPlatform = new MacOSX.clsMacOSXPlatform();
			//	} else {
			//		IsLinux = true;
			//		sThisPlatform = new Linux.clsLinuxPlatform();
			//	}

			//} else {
			//	IsWindows = true;
			//	sThisPlatform = new Windows.clsWinPlatform();
			//}

        }

        /// <summary>
        /// Retrives the current platform with functions handling platform specific differences.
        /// </summary>
        [DebuggerNonUserCode()]
        public static absTSMPlatform TSMPlatform
        {
            get
            {
                return sThisPlatform;
            }
        }

    }
}
