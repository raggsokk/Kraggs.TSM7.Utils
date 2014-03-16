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
    }
}
