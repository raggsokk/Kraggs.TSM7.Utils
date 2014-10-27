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

namespace Kraggs.TSM7.Utils
{
    /// <summary>
    /// The dsmadmc exit code is shown in the following table and can be one of three severities: Ok, Warning and Error.
    /// </summary>
    public enum AdmcExitCode
    {
        /// <summary>
        /// The command completed successfully. Severity: Ok (doh!)
        /// </summary>
        Ok = 0,

        /// <summary>
        /// The command is not found, not a known command. Severity: Error.
        /// </summary>
        Unknown = 2,

        /// <summary>
        /// The command is valid, but one or more parameters were not specified correctly. Severity: Error.
        /// </summary>
        Syntax = 3,

        /// <summary>
        /// An internal server error prevented the command from successfully completing. Severity: Error.
        /// </summary>
        Error = 4,

        /// <summary>
        /// The command could not be completed because of insufficient memory on the server. Severity: Error.
        /// </summary>
        NoMemory = 5,

        /// <summary>
        /// The command could not be completed because of insufficient recovery log space on the server. Severity: Error.
        /// </summary>
        NoLog = 6,

        /// <summary>
        /// The command could not be completed becase of insufficient database space on the server. Severity: Error.
        /// </summary>
        NoDB = 7,

        /// <summary>
        ///  The command could not be completed because of insufficient storage space on the server. Severity: Error.
        /// </summary>
        NoStorage = 8,

        /// <summary>
        /// The command failed because the administrator is not authorized to issue the command. Severity: Error.
        /// </summary>
        NoAuth = 9,

        /// <summary>
        /// The command failed because the specified object already exists on the server. Severity: Error.
        /// </summary>
        Exists = 10,

        /// <summary>
        /// Returned by a Query or SQL Select command when no objects are found that match specifications. Severity: Warning.
        /// </summary>
        NotFound = 11,

        /// <summary>
        /// The command failed because the object to be operated upon was in use. Severity: Error.
        /// </summary>
        InUse = 12,

        /// <summary>
        /// The command failed because the object to be operated upon is still referenced by some other server constructs. Severity: Error.
        /// </summary>
        IsReferenced = 13,

        /// <summary>
        /// The command failed because the object to be operated upon is not available. Severity: Error.
        /// </summary>
        NotAvailable = 14,

        /// <summary>
        /// The command failed because an input/output (I/O) error was encountered on the server. Severity: Error.
        /// </summary>
        IOError = 15,

        /// <summary>
        /// The command failed because a database transaction failed on the server.
        /// </summary>
        NoTXN = 16,

        /// <summary>
        /// The command failed because a lock conflict was encountered in the server database. Severity: Error.
        /// </summary>
        NoLock = 17,

        /// <summary>
        /// The command could not be completed because of insufficient memory on the server. Severity: Error.
        /// </summary>
        NoThread = 19,

        /// <summary>
        /// The command failed because the server is not in compliance with licensing. Severity: Error.
        /// </summary>
        License = 20,

        /// <summary>
        /// The command failed because a destination value was invalid. Severity: Error.
        /// </summary>
        InvDest = 21,

        /// <summary>
        /// The command failed because an input file that was needed could not be opened. Severity: Error.
        /// </summary>
        IFileOpen = 22,

        /// <summary>
        /// The command failed because it could not open a required output file. Severity: Error.
        /// </summary>
        OFileOpen = 23,

        /// <summary>
        /// The command failed because it could not successfully write to a required output file. Severity: Error.
        /// </summary>
        OFileWrite = 24,

        /// <summary>
        /// The command failed because the administrator was not defined. Severity: Error.
        /// </summary>
        InvAdmin = 25,

        /// <summary>
        /// An SQL error was encountered during a SELECT statement query. Severity: Error.
        /// </summary>
        SqlError = 26,

        /// <summary>
        /// The command failed because the command is used in an invalid manner. Severity: Error.
        /// </summary>
        InvalidUse = 27,

        /// <summary>
        /// The command failed because of an unknown SQL table name. Severity: Error.
        /// </summary>
        NoTable = 28,

        /// <summary>
        /// The command failed because of Non-Compatible Filespace name types. Severity: Error.
        /// </summary>
        FSNotCap = 29,

        /// <summary>
        /// The command failed because of an incorrect high-level address or low-level address. Severity: Error.
        /// </summary>
        InvalidAddr = 30,

        /// <summary>
        /// The command failed because the management class does not have an archive copy group. Severity: Error.
        /// </summary>
        InvalidCG = 31,

        /// <summary>
        /// The command failed because the volume size exceeds the maximum allowed. Severity: Error.
        /// </summary>
        OversizeVol = 32,

        /// <summary>
        /// The command failed because volumes cannot be defined in RECLAMATIONTYPE=SNAPLOCK storage pools. Severity: Error.
        /// </summary>
        DefVolFail = 33,

        /// <summary>
        /// The command failed because volumes cannot be deleted in RECLAMATIONTYPE=SNAPLOCK storage pools. Severity: Error.
        /// </summary>
        DelVolFail = 34,

        /// <summary>
        /// The command is canceled. Severity: Warning.
        /// </summary>
        Canceled = 35,

        /// <summary>
        /// The command failed because there is an invalid definition in the policy domain. Severity: Error.
        /// </summary>
        InvPolicy = 36,

        /// <summary>
        /// The command failed because of an invalid password. Severity: Error.
        /// </summary>
        InvalidPW = 37,

        /// <summary>
        /// The command failed because the command or the parameter is not supported. Severity: Warning.
        /// </summary>
        UnsuppParm = 38,

        /// <summary>
        /// Admin commands on client port is disabled. Specify an admin port instead.
        /// Not a dsmadmc defined code...
        /// </summary>
        AdminOnClientPort = 77,

        /// <summary>
        /// Not a dsmadmc defined code, but indicates external or environmental failure to run binary.
        /// Example: 
        /// ANS1035S Options file 'C:\Program Files\Tivoli\TSM\baclient\dsm.opt' could not be found, or it cannot be read.
        /// Not write access to dsmerror.log
        /// </summary>
        FailedToExecute = -1,

        /// <summary>
        /// ANS1033E An invalid TCP/IP address was specified.
        /// Not a dsmadmc defined code...
        /// </summary>
        InvalidTCPIPAddress = -53,
        
    }
}
