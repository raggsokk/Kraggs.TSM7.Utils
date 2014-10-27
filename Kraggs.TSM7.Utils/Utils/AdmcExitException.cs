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
using System.Linq;

namespace Kraggs.TSM7.Utils
{
    public class AdmcExitException : TSMException // : Exception
    {
        public AdmcExitCode ExitCode { get; protected set; }
        //public clsTSMMessage TSMMessage { get; protected set; }

        //public AdmcExitException(AdmcExitCode exitCode, clsTSMMessage message)
        public AdmcExitException(AdmcExitCode exitCode, string message = null)
            : this(exitCode, message != null ? new List<string>() { message} : null)
        {
            //throw new NotImplementedException();
            // dummy!.
        }

        public AdmcExitException(AdmcExitCode exitCode, List<string> output = null)
        {
            this.ExitCode = exitCode;
            this.DecodeMessage(output);

            //if(output != null)
            //{
            //    this.DecodeMessage(output);

            //    //clsTSMMessage msg;
            //    //var len = output.Count;
            //    //if(len == 0)
            //    //    return;
                
            //    //var last = output[len - 1];
            //    //var almostlast = len > 1 ? output[len - 2] : string.Empty;

            //    //if (clsTSMMessage.TryParse(almostlast, out msg))
            //    //    this.TSMMessage = msg;
            //    //else
            //    //{
            //    //    if (clsTSMMessage.TryParse(last, out msg))
            //    //        this.TSMMessage = msg;
            //    //}
            //}
        }

        //public TSMSeverity Severity
        //{
        //    get
        //    {
        //        return TSMMessage.Severity;
        //    }
        //}
    }
}
