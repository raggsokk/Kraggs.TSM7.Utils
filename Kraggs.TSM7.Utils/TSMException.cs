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

using System.Diagnostics;
//using System.Runtime.Serialization;

namespace Kraggs.TSM7
{
    /// <summary>
    /// Generic TSM Exception. 
    /// </summary>
    public class TSMException : Exception
    {
        public clsTSMMessage TSMMessage { get; protected set; }

        //public List<clsTSMMessage> TSMMessages { get; set; }

        protected TSMException() : base()
        {
            // dummy!
        }

        public TSMException(string message) : base(message)
        {
            this.DecodeMessage(message);
        }

        /// <summary>
        /// Tries to decode a TSM Message out of list of output.
        /// Actually only tries last 2 lines.
        /// DOES NOT SET BASE MESSAGE!
        /// </summary>
        /// <param name="output"></param>
        public TSMException(List<string> output) : this()
        {
            this.DecodeMessage(output);
        }

        //protected TSMException( SerializationInfo info, StreamingContext context ) : base( info, context ) 
        //{ 
        //    if(info != null)
        //    {
        //    //    this.ExitCode = info.GetInt32("ExitCode");
        //    //    this.TSMMessages = (List<clsTSMMessage>)info.GetValue("TSMMessages", typeof(List<clsTSMMessage>));
        //        //var msg = info.GetValue("TSMMessage", typeof(clsTSMMessage)) as clsTSMMessage;
        //        //if (msg != null)
        //        //    this.TSMMessage = msg;
        //        this.TSMMessage = info.GetValue("TSMMessage", typeof(clsTSMMessage)) as clsTSMMessage;
        //    }
        //}

        /// <summary>
        /// Tries to decode message string into a TSMMessage class.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual bool DecodeMessage(string message)
        {
            // could have decoded inhouse but this way code path is unified.
            return DecodeMessage(new List<string>() { message });
        }

        /// <summary>
        /// Tries to decode the error message inside message list.
        /// Actually only tries 2 last items...
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        protected virtual bool DecodeMessage(List<string> messages)
        {
            if (messages == null)
                return false;

            var count = messages.Count;

            if(count == 0)
                return false;
            if(count == 1)
            {
                clsTSMMessage msg;
                if(clsTSMMessage.TryParse(messages[0], out msg))
                {
                    this.TSMMessage = msg;
                    return true;
                }
                //return false;
            }
            else
            {
                // get last 2 elements in list.
                // Yay, a linq call!
                //var last = messages.Skip(Math.Max(0, count - 2)).Take(2);
                var last = messages.Skip(count - 2);
                //var last = messages.Reverse<string>().Take(2); this reverts the whole array. This can be very big so dont.
                
                if(last != null)
                {
                    Debug.Assert(last.Count() == 2, string.Format("Code to get last 2 items in list failed. Result: {0}", last.Count()));

                    // try second last first. 
                    clsTSMMessage msg = null;
                    if (clsTSMMessage.TryParse(last.First(), out msg))
                        this.TSMMessage = msg;
                    else if (clsTSMMessage.TryParse(last.Last(), out msg))
                        this.TSMMessage = msg;

                    if (TSMMessage != null)
                        return true;
                }
            }

            return false;
        }

		public override string ToString()
		{
			//http://stackoverflow.com/questions/1886611/c-overriding-tostring-method-for-custom-exceptions
			//TODO: Enable inheritors to Append their data too.

			var sb = new StringBuilder();
			sb.AppendFormat("{0}: ", this.GetType().Name);

			if(TSMMessage != null)
				sb.Append(TSMMessage.FullMessage);
			else
				sb.Append(Message);

			if(this.InnerException != null) {
				sb.AppendFormat(" ---> {0}", InnerException);
				sb.AppendFormat(
					"{0}   --- End of inner exception stack trace ---{0}", Environment.NewLine);
			}

			sb.Append(this.StackTrace);

			return sb.ToString();
			//return string.Format("[TSMException: TSMMessage={0}]", TSMMessage);
		}
    }
}
