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
using System.Runtime.Serialization;

namespace Kraggs.TSM7
{
    public class clsTSMException : Exception
    {
        public int ExitCode { get; set; }

        public List<clsTSMMessage> TSMMessages { get; set; }

        protected clsTSMException()
        {
            this.TSMMessages = new List<clsTSMMessage>();
            this.ExitCode = -1;
        }

        public clsTSMException(int exitCode) : this()
        {
            this.ExitCode = exitCode;
        }

        //public clsTSMException(clsTSMMessage message) : this(new List<clsTSMMessage>(){message})
        //{

        //}
        public clsTSMException(List<clsTSMMessage> messages) : this()
        {
            this.TSMMessages = messages;
        }

        public clsTSMException(List<string> messages) : this()
        {
            clsTSMMessage msg;

            foreach(var s in messages)
            {
                if (clsTSMMessage.TryParse(s, out msg))
                    this.TSMMessages.Add(msg);
            }
        }
        protected clsTSMException( SerializationInfo info, StreamingContext context ) : base( info, context ) 
        { 
            if(info != null)
            {
                this.ExitCode = info.GetInt32("ExitCode");
                this.TSMMessages = (List<clsTSMMessage>)info.GetValue("TSMMessages", typeof(List<clsTSMMessage>));
            }
        }
    }
}
