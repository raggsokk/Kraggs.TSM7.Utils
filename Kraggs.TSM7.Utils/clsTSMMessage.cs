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
    /// <summary>
    /// A class for working with TSM Messages.
    /// A TSM Message is not the same as an TSM Error Code or return code.
    /// http://pic.dhe.ibm.com/infocenter/tsminfo/v7r1/topic/com.ibm.itsm.nav.doc/msgs_msgsformat.html
    /// </summary>
    public class clsTSMMessage
    {
        /// <summary>
        /// A three-letter prefix. Messages have different prefixes to help you identify the Tivoli Storage Manager component that issues the message. Typically, all messages for a component have the same prefix. Sometimes a component issues messages with two or three different prefixes
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// A numeric message identifier
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Type/Severity of message.
        /// S=Severe,The product or a product function cannot continue. User response is required.
        /// E=Error,An error is encountered during processing. Processing might stop. User response might be required.
        /// W=Warning,Processing continues, but problems might occur later as a result of the warning.
        /// I=Information,Processing continues. User response is not necessary.
        /// </summary>
        public char Type { get; set; }

        /// <summary>
        /// Message text that is displayed on screen and written to message logs.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Assembles the FullMessage.
        /// </summary>
        public string FullMessage
        {
            get
            {
                return string.Format("{0}{1}{2} {3}",
                    Prefix, Number, Type, Text);
            }
        }
        
        /// <summary>
        /// A little default ctor protected so that it cant be created except those below.
        /// </summary>
        [DebuggerNonUserCode()]
        protected clsTSMMessage()
        { }

        /// <summary>
        /// Serialized constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected clsTSMMessage(SerializationInfo info, StreamingContext  context)
        {
            if(info != null)
            {
                this.Prefix = info.GetString("Prefix");
                this.Number = info.GetInt32("Number");
                this.Type = info.GetChar("Type");
                this.Text = info.GetString("Text");
            }
        }

        /// <summary>
        /// Try parse a string into an message string.
        /// </summary>
        /// <param name="MessageString"></param>
        /// <param name="tsmmsg"></param>
        /// <returns></returns>
        public static bool TryParse(string MessageString, out clsTSMMessage tsmmsg)
        {
            tsmmsg = null;

            if (!Char.IsLetter(MessageString[0]))
                return false;
            if (!Char.IsLetter(MessageString[1]))
                return false;
            if (!Char.IsLetter(MessageString[2]))
                return false;

            var prefix = MessageString.Substring(0, 3).ToUpperInvariant();

            int number = 0;
            if (!int.TryParse(MessageString.Substring(3, 4), out number))
                return false;

            var severity = MessageString[7];

            if (!Char.IsLetter(severity))
                return false;

            string text = string.Empty;

            if (MessageString.Length > 9)
                text = MessageString.Substring(9);

            tsmmsg = new clsTSMMessage()
            {
                Prefix = prefix,
                Number = number,
                Type = severity,
                Text = text,
            };

            return true;
        }

    }
}
