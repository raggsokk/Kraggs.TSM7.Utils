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

        static clsTSMPlatform()
        {
            //if (Environment.OSVersion.Platform == PlatformID.Unix)
            //    sThisPlatform = new Linux.clsLinuxPlatform();
            //else if (Environment.OSVersion.Platform == PlatformID.MacOSX)
            //    sThisPlatform = new Linux.clsLinuxPlatform(); // for now reuse this.
            //else
            //    sThisPlatform = new Windows.clsWinPlatform();

            if (IsLinux)
                sThisPlatform = new Linux.clsLinuxPlatform();
            else if (IsMacOSX)
                sThisPlatform = new Linux.clsLinuxPlatform(); // for now reuse this.
            else
                sThisPlatform = new Windows.clsWinPlatform();
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

        /// <summary>
        /// Helper function for determing which os we are on.
        /// </summary>
        [DebuggerNonUserCode()]
        public static bool IsLinux
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.Unix;
            }
        }

        /// <summary>
        /// Helper function for determing which os we are on.
        /// </summary>
        [DebuggerNonUserCode()]
        public static bool IsMacOSX
        {
            get
            {
                return Environment.OSVersion.Platform == PlatformID.MacOSX;
            }
        }

        /// <summary>
        /// Helper function for determing which os we are on.
        /// </summary>
        [DebuggerNonUserCode()]
        public static bool IsWindows
        {
            get
            {
                // lazy hack.
                return !IsMacOSX && !IsLinux;
            }
        }
    }
}
