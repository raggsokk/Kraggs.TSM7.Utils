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

namespace Kraggs.TSM7
{
    /// <summary>
    /// 
    /// </summary>
    public enum TSMSeverity
    {
        /// <summary>
        /// Invalid value.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Information,Processing continues. User response is not necessary.
        /// </summary>
        Information = 'I',
        /// <summary>
        /// Warning,Processing continues, but problems might occur later as a result of the warning.
        /// </summary>
        Warning = 'W',
        /// <summary>
        /// Error, An error is encountered during processing. Processing might stop. User response might be required.
        /// </summary>
        Error = 'E',
        /// <summary>
        /// Severe,The product or a product function cannot continue. User response is required.
        /// </summary>
        Fatal = 'S',
    }
}
