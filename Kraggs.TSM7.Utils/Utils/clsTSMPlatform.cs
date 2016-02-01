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
			var osplatform = Environment.OSVersion.Platform;

			if(osplatform == PlatformID.MacOSX) {
				IsMacOSX = true;
				sThisPlatform = new MacOSX.clsMacOSXPlatform();
			} else if(osplatform == PlatformID.Unix) {

				// this can still be maxosx unfortunately.

				var flagRunningMac = MacOSX.clsMacOSXPlatform.GetIsRunningMac();

				if(flagRunningMac) {
					IsMacOSX = true;
					sThisPlatform = new MacOSX.clsMacOSXPlatform();
				} else {
					IsLinux = true;
					sThisPlatform = new Linux.clsLinuxPlatform();
				}

			} else {
				IsWindows = true;
				sThisPlatform = new Windows.clsWinPlatform();
			}

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
