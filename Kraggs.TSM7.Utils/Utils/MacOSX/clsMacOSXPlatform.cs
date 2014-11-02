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

using System.Runtime.InteropServices;

namespace Kraggs.TSM7.Utils.MacOSX
{
	/// <summary>
	/// TSM Platform code for Mac OSX.
	/// </summary>
	internal class clsMacOSXPlatform : absTSMPlatform
	{
		//internal static readonly string DEFAULT_PATH = @"/opt/tivoli/tsm/client/ba/bin";
		internal static readonly string DEFAULT_PATH = @"/Library/Application Support/tivoli/tsm/client/ba/bin";
		internal string pMacPath;// = @"/opt/tivoli/tsm/client/ba/bin";
		internal string pMacDsmc;// = Path.Combine(sLinuxPath, "dsmc");
		internal string pMacDsmAdmc;// = Path.Combine(sLinuxPath, "dsmadmc");

		public clsMacOSXPlatform ()
		{
			if (Directory.Exists (DEFAULT_PATH))
				pMacPath = DEFAULT_PATH;
			else
				return; 

			var testpath = Path.Combine(pMacPath, "dsmc");
			if(File.Exists(testpath))
				pMacDsmc = testpath;

			testpath = Path.Combine(pMacPath, "dsmadmc");
			if(File.Exists(testpath))
				pMacDsmAdmc = testpath;
				
		}

		//From Managed.Windows.Forms/XplatUI
		[DllImport("libc")]
		private static extern int uname(IntPtr buf);

		public static bool GetIsRunningMac()
		{
			var buf = IntPtr.Zero;
			try
			{
				buf = Marshal.AllocHGlobal(8192);

				if(uname(buf) == 0)
				{
					string osname = Marshal.PtrToStringAnsi(buf);
					if(osname == "Darwin")
						return true;
				}
			}
			catch {
			}
			finally{
				if(buf != IntPtr.Zero)
					Marshal.FreeHGlobal(buf);
			}

			return false;
		}

		#region implemented abstract members of absTSMPlatform

		/// <summary>
		/// Returns the full path to the default BAClient install path on current platform.
		/// </summary>
		/// <value>The BA client path.</value>
		[DebuggerNonUserCode()]
		public override string BAClientPath {
			get {
				return pMacPath;
			}
		}

		/// <summary>
		/// Returns the full path to the default dsmc binary on current platform.
		/// </summary>
		/// <value>The dsmc binary.</value>
		[DebuggerNonUserCode()]
		public override string DsmcBinary {
			get {
				return pMacDsmc;
			}
		}

		/// <summary>
		/// Returns the full path to the default dsmadmc binary on current platform.
		/// </summary>
		/// <value>The dsm admc binary.</value>
		[DebuggerNonUserCode()]
		public override string DsmAdmcBinary {
			get {
				return pMacDsmAdmc;
			}
		}

		#endregion
	}
}

