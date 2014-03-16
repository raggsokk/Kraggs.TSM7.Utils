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

namespace Kraggs.TSM7.Utils.Linux
{
    /// <summary>
    /// TSM Platform code for Linux.
    /// </summary>
    internal class clsLinuxPlatform : absTSMPlatform
    {
        //internal static readonly string sLinuxPath = @"/opt/tivoli/tsm/client/ba/bin";
        //internal static readonly string sLinuxDsmc = Path.Combine(sLinuxPath, "dsmc");
        //internal static readonly string sLinuxDsmAdmc = Path.Combine(sLinuxPath, "dsmadmc");
        internal static readonly string DEFAULT_PATH = @"/opt/tivoli/tsm/client/ba/bin";
        internal string pLinuxPath;// = @"/opt/tivoli/tsm/client/ba/bin";
        internal string pLinuxDsmc;// = Path.Combine(sLinuxPath, "dsmc");
        internal string pLinuxDsmAdmc;// = Path.Combine(sLinuxPath, "dsmadmc");

        public clsLinuxPlatform()
        {
            if (Directory.Exists(DEFAULT_PATH))
                this.pLinuxPath = DEFAULT_PATH;

            var testpath = Path.Combine(DEFAULT_PATH, "dsmc");
            if (File.Exists(testpath))
                pLinuxDsmc = testpath;

            testpath = Path.Combine(DEFAULT_PATH, "dsmadmc");
            if (File.Exists(testpath))
                pLinuxDsmAdmc = testpath;
        }

        [DebuggerNonUserCode()]
        public override string DsmcBinary
        {
            get
            {
                return pLinuxDsmc;
            }
        }

        [DebuggerNonUserCode()]
        public override string DsmAdmcBinary
        {
            get { return pLinuxDsmAdmc; }
        }

        [DebuggerNonUserCode()]
        public override string BAClientPath
        {
            get { return pLinuxPath; }
        }

        /// <summary>
        /// Returns the dsm.sys location on linux.
        /// </summary>
        [DebuggerNonUserCode()]
        public override string DsmSys
        {
            get { return Path.Combine(pLinuxPath, "dsm.sys"); }
        }
    }
}
