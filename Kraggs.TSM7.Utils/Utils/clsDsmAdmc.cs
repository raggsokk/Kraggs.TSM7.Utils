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

namespace Kraggs.TSM7.Utils
{
    /// <summary>
    /// A somewhat simple DsmAdmc binary wrapper.
    /// </summary>
    public class clsDsmAdmc : absProcess
    {
        // Connection info.
        protected string pServer;
        protected string pUsername;
        protected string pPassword;
        protected int? pPort;

        protected string pOptFile;

        /// <summary>
        /// Throws exception on tsm errors. argument exception will still occur.
        /// </summary>
        public bool ThrowOnErrors { get; protected set; }

        protected clsDsmAdmc() : base()
        {
            // Not particular satisfied with this solution but it works for now...

            this.pExecutable = clsTSMPlatform.TSMPlatform.DsmAdmcBinary;
            this.pWorkingDir = clsTSMPlatform.TSMPlatform.BAClientPath;

            this.pOptFile = clsTSMPlatform.TSMPlatform.DsmOpt;

            this.Validate();
        }

        /// <summary>
        /// Sets up 
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <param name="OptFile"></param>
        public clsDsmAdmc(string Username, string Password, string OptFile = null) : this()
        {
            this.pUsername = Username;
            this.pPassword = Password;

            if(!string.IsNullOrWhiteSpace(OptFile))
                this.pOptFile = OptFile;
        }

        public clsDsmAdmc(string Username, string Password, string Server, int? Port = null, string OptFile = null)
            : this()
        {
            //TODO: Validate username / server. its easy to switch wrong....
            this.pUsername = Username;
            this.pPassword = Password;
            this.pServer = Server;
            this.pPort = Port;

            if (!string.IsNullOrWhiteSpace(OptFile))
                this.pOptFile = OptFile;
        }

        //Doesn't have any error handling.
        //public AdmcExitCode RunTSMCommandToCallback(string tsmCommand, DataReceivedEventHandler output = null, int TimeoutMs = 0 )
        //{
        //    var sb = GenerateStandardArguments();

        //    sb.AppendFormat(" \"{0}\"", tsmCommand);
           
        //    var retcode = (AdmcExitCode)base.RunCommand(sb.ToString(), output, null, TimeoutMs);

        //    if (!ThrowOnErrors)
        //    {
        //        if (retcode != AdmcExitCode.Ok && retcode != AdmcExitCode.NotFound)
        //        {
        //            if (output != null && output.Count > 0)
        //            {
        //                throw new Exception(output[0]);
        //            }
        //            else //TODO: the error migth been redirected to outfile also. Check for error there?
        //                throw new Exception("An unspecified error occured. Check with dsmerror.log for more info.");
        //        }
        //    }

        //    return retcode;
        //}

        /// <summary>
        /// Runs a tsm macro and redirects output to a string list.
        /// </summary>
        /// <param name="macrofile"></param>
        /// <param name="output"></param>
        /// <param name="TimeoutMs">Optional timeout for tsm call.</param>
        /// <returns></returns>
        public AdmcExitCode RunTSMMacroToList(string macrofile, List<string> output = null, int TimeoutMs = 0 )
        {
            var sb = GenerateStandardArguments();

            sb.AppendFormat(" macro={0}", macrofile);

            return RunTSMCommand(sb.ToString(), output, TimeoutMs);
        }

        /// <summary>
        /// Runs a tsm macro and pipes the result to a specified output file.
        /// </summary>
        /// <param name="macrofile"></param>
        /// <param name="OutputFile"></param>
        /// <param name="TimeoutMs">Optional timeout for tsm call.</param>
        /// <returns></returns>
        public AdmcExitCode RunTSMMacroToFile(string macrofile, string OutputFile, int TimeoutMs = 0 )
        {
            var sb = GenerateStandardArguments(OutFile: OutputFile);

            sb.AppendFormat(" macro={0}", macrofile);

            var dummylist = new List<string>(); // to enable error catching only.

            return RunTSMCommand(sb.ToString(), dummylist, TimeoutMs);
        }

        /// <summary>
        /// Runs a tsm command and redirects output to string list.
        /// </summary>
        /// <param name="tsmCommand">Command to run.</param>
        /// <param name="output">list catching result of command, or null if not required.</param>
        /// <param name="TimeoutMs">Optional timeout for tsm call.</param>
        /// <returns></returns>
        public AdmcExitCode RunTSMCommandToList(string tsmCommand, List<string> output = null, int TimeoutMs = 0 )
        {
            var sb = GenerateStandardArguments();

            sb.AppendFormat(" \"{0}\"", tsmCommand);

            return RunTSMCommand(sb.ToString(), output, TimeoutMs);
        }

        /// <summary>
        /// Runs a tsm command and pipes result to a specified file.
        /// </summary>
        /// <param name="tsmCommand">TSM Command to run.</param>
        /// <param name="OutputFile">File pipe tsm result to.</param>
        /// <param name="TimeoutMs">Optional timeout for tsm call.</param>
        /// <returns></returns>
        public AdmcExitCode RunTSMCommandToFile(string tsmCommand, string OutputFile, int TimeoutMs = 0)
        {

            var sb = GenerateStandardArguments(OutFile: OutputFile);

            sb.AppendFormat(" \"{0}\"", tsmCommand);

            var dummylist = new List<string>(); // to enable error catching only.

            return RunTSMCommand(sb.ToString(), dummylist, TimeoutMs);
        }
        /// <summary>
        /// Runs a tsm command and pipes result to a temp file. Result is then read into output unless errors occured.
        /// </summary>
        /// <param name="tsmCommand">TSM Command to run.</param>
        /// <param name="output">Result of tsm command read from temp file.</param>
        /// <param name="TimeoutMs">Optional timeout for tsm call.</param>
        /// <returns></returns>
        public AdmcExitCode RunTSMCommandToFile(string tsmCommand, List<string> output, int TimeoutMs = 0)
        {
            string tmpfile = null;

            if (output == null)
                throw new ArgumentNullException("output");

            try
            {
                tmpfile = Path.GetTempFileName();

                var retcode = RunTSMCommandToFile(tsmCommand, tmpfile, TimeoutMs);

                if(File.Exists(tmpfile))
                {
                    // NOT Very efficient but works for now.
                    var lines = File.ReadAllLines(tmpfile);
                    
                    foreach (var l in lines)
                        output.Add(l);
                }

                return retcode;
            }
            finally
            {
                if (File.Exists(tmpfile))
                    File.Delete(tmpfile);
            }            
        }

        /// <summary>
        /// Common RunCommand wrapper for dsmadmc commands.
        /// </summary>
        /// <param name="FinishedArgument"></param>
        /// <param name="output"></param>
        /// <param name="TimeoutMs"></param>
        /// <returns></returns>
        protected virtual AdmcExitCode RunTSMCommand(string FinishedArgument, List<string> output, int TimeoutMs = 0)
        {
            var retcode = (AdmcExitCode)base.RunCommand(FinishedArgument, output, null, TimeoutMs);

            if (!ThrowOnErrors)
            {
                if (retcode != AdmcExitCode.Ok && retcode != AdmcExitCode.NotFound)
                {
                    if (output != null && output.Count > 0)
                    {
                        throw new Exception(output[0]);
                    }
                    else //TODO: the error migth been redirected to outfile also. Check for error there?
                        throw new Exception("An unspecified error occured. Check with dsmerror.log for more info.");
                }
            }

            return retcode;
        }

        /// <summary>
        /// Generates the various arguments usualy used when calling tsm like this.
        /// </summary>
        /// <param name="DataOnly">Skip Header and footer in admadmc calls.</param>
        /// <param name="CsvFormat">Format data like csv.</param>
        /// <param name="NoConfirm">Dont confirm changes.</param>
        /// <param name="ItemCommit">Commit commands in a macro line by line.</param>
        /// <param name="OutFile">Pipe result from command to this file.</param>
        /// <param name="OptFile">Optionally use options in this file, for options not already specified.</param>
        /// <returns></returns>
        protected virtual StringBuilder GenerateStandardArguments(bool DataOnly = true, bool CsvFormat = true, bool NoConfirm = false, bool ItemCommit = false, string OutFile = null, string OptFile = null)
        {
            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(this.pServer))
            {
                if (clsTSMPlatform.IsLinux)
                    sb.AppendFormat(" -se={0}", pServer);
                else
                {
                    sb.AppendFormat(" -tcps={0}", pServer);

                    if (pPort.HasValue && pPort.Value > 0)
                        sb.AppendFormat(" -tcpp={0}", pPort.Value);
                }
            }

            sb.AppendFormat(" -id={0} -pa={1}", pUsername, pPassword);

            if (DataOnly)
                sb.Append(" -dataonly=yes");
            if (CsvFormat)
                sb.Append(" -comma");
            if (NoConfirm)
                sb.Append(" -NoConfirm");
            if (ItemCommit)
                sb.Append(" -ItemCommit");

            if (!string.IsNullOrWhiteSpace(OptFile))
                sb.AppendFormat(" -optfile=\"{0}\"", OptFile);
            else if(!string.IsNullOrWhiteSpace(this.pOptFile))
                sb.AppendFormat(" -optfile=\"{0}\"", this.pOptFile);

            if (!string.IsNullOrWhiteSpace(OutFile))
                sb.AppendFormat(" -OUTfile=\"{0}\"", OutFile);

            return sb;
        }
    }
}
