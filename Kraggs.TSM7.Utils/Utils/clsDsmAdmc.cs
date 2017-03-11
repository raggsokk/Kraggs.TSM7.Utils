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
    /// TODO:
    ///     Check size of dsmerror.log and throw warning 
    /// DONE:
    ///     Check existance of dsm.opt and throw error if not.
    ///     Optionally save last command executed for debuging purposes.
    ///     Found workaround for "Check write access to dsmerror.log beforehand. and throw error."
    /// </summary>
    public class clsDsmAdmc : absProcess
    {
        // Connection info.
        /// <summary>
        /// The ip or hostname of the TSM Server to connect to.
        /// </summary>
        public string Server { get; protected set; }
        /// <summary>
        /// The Admin Username to use connect as.
        /// </summary>
        public string Username { get; protected set; }
        /// <summary>
        /// The Admin Password to use during connection.
        /// </summary>
        protected string pPassword;
        /// <summary>
        /// Optionally the admin port to connect to in case of non-default TSM Admin Port.
        /// Very seldom needed thou.
        /// </summary>
        public int? Port { get; protected set; }

        /// <summary>
        /// In case custom option file is in use, store this here.
        /// </summary>
        protected string pOptFile;

        /// <summary>
        /// Throws exception on tsm errors. argument exception will still occur.
        /// </summary>
        public bool ThrowOnErrors { get; protected set; } = true;

        protected clsDsmAdmc() : base()
        {
            // Not particular satisfied with this solution but it works for now...
            var platform = clsTSMPlatform.TSMPlatform;
            Debug.Assert(platform != null, "TSMPlatform is null, should always have been set in type init.");

            this.pExecutable = platform.DsmAdmcBinary;
            this.pWorkingDir = platform.BAClientPath;

            // this.pOptFile = clsTSMPlatform.TSMPlatform.DsmOpt;

            // required by absProcess.
            this.Validate();

            //TODO: combine our validation with absProcess.Validate somehow?
            // validate our dsmadmc running environment.
            platform.ValidatePlatform();

            // setting up DSM_LOG to bypass dsmerror.log errors even if non-admin non-elevated.
            this.pEnvironments.Add("DSM_LOG", platform.GetUserWritableDirectory());            
        }

        /// <summary>
        /// Sets up a dsmadmc class with a username and password to specified server.
        /// Optionally also sets up a port to connect to or/and custom optional file.
        /// </summary>
        /// <param name="Username">The Admin Username-</param>
        /// <param name="Password">The Admin Password.</param>
        /// <param name="Server">The Ip or Hostname of the TSM Server to connect to.</param>
        /// <param name="Port">Optionally the tsm server port to connect to.</param>
        /// <param name="OptFile">Optionally, uses a custom Option File during connection.</param>
        public clsDsmAdmc(string Username, string Password, string Server, int? Port = null, string OptFile = null)
            : this()
        {
            //TODO: Validate username / server. its easy to switch wrong....
            this.Username = Username;
            this.pPassword = Password;
            this.Server = Server;
            this.Port = Port;

            if (!string.IsNullOrWhiteSpace(OptFile))
                this.pOptFile = OptFile;
        }

        /// <summary>
        /// Connects to default tsm server and port using specified Admin Username and password.
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
		public static clsDsmAdmc CreateWithDefaults(string Username, string Password)
		{
			var admc = new clsDsmAdmc();
			admc.Username = Username;
			admc.pPassword = Password;

			return admc;
		}

		/// <summary>
		/// Creates a dsmadmc with username, password and custom optionfile. 
		/// </summary>
		/// <returns>The with option file.</returns>
		/// <param name="Username">Username to connect to tsm server as.</param>
		/// <param name="Password">Password to connect to tsm server as.</param>
		/// <param name="OptionFile">Option file with connection parameters. For Unix see remarks.</param>
		/// <remarks>
		/// Although the linux/mac client can use custom option file with '-optfile' there are some rules to its usage.
		/// In the same directory as the customer optionfile parameter this must be supplied.
		/// 	1. dsm.sys : Doesn't matter what the optionfile is named, a dsm.sys file must exist with matching server stanza.
		/// 	2. libs    : The shared libs needed to run dsmadmc must be copied or symlinked to this folder.
		/// 	3. working dir: The working directory must be set to the same path.
		/// 		(which currently is not possible).
		/// </remarks>
		public static clsDsmAdmc CreateWithOptionFile(string Username, string Password, string OptionFile)
		{
			if(!File.Exists(OptionFile))
				throw new FileNotFoundException("OptionFile", OptionFile);
				
			var admc = new clsDsmAdmc();
			admc.Username = Username;
			admc.pPassword = Password;
			admc.pOptFile = OptionFile;

			return admc;
		}

		/// <summary>
		/// Creates a dsmadmc with username, password and server. 
		/// </summary>
		/// <returns>The with server.</returns>
		/// <param name="Username">Username to connect to tsm server as.</param>
		/// <param name="Password">Password to connect to tsm server as.</param>
		/// <param name="Server">On Windows, ip or hostname. On Unix this is ServerStanza.</param>
		/// <param name="Port">On Windows optionally port to connect to. On Unix this is ignored.</param>
		public static clsDsmAdmc CreateWithServer(string Username, string Password, string Server, int? Port)
		{
			var admc = new clsDsmAdmc();
			admc.Username = Username;
			admc.pPassword = Password;
			admc.Server = Server;
			admc.Port = Port;

			return admc;
		}


        //Doesn't have any error handling. But now it does.
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

            //var dummylist = new List<string>(); // to enable error catching only. Dont need anymore.

            return RunTSMCommand(sb.ToString(), null, TimeoutMs);
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

            //var dummylist = new List<string>(); // to enable error catching only. Dont need anymore.

            return RunTSMCommand(sb.ToString(), null, TimeoutMs);
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
            var list = output != null ? output : new List<string>();

            //var retcode = (AdmcExitCode)base.RunCommand(FinishedArgument, output, null, TimeoutMs);
            var retcode = (AdmcExitCode)base.RunCommand(FinishedArgument, list, null, TimeoutMs);

            if (ThrowOnErrors)
            {
                if (retcode != AdmcExitCode.Ok && retcode != AdmcExitCode.NotFound)
                {
                    throw new AdmcExitException(retcode, list);
                    //if (list != null && list.Count > 0)
                    //{
                    //    throw new Exception(list[0]);
                    //}
                    //else //TODO: the error migth been redirected to outfile also. Check for error there?
                    //    throw new Exception("An unspecified error occured. Check with dsmerror.log for more info.");
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

            if (!string.IsNullOrWhiteSpace(this.Server))
            {
                if (clsTSMPlatform.IsLinux || clsTSMPlatform.IsMacOSX)
                    sb.AppendFormat(" -se={0}", Server);
                else
                {
                    sb.AppendFormat(" -tcps={0}", Server);

                    if (Port.HasValue && Port.Value > 0)
                        sb.AppendFormat(" -tcpp={0}", Port.Value);
                }
            }

            sb.AppendFormat(" -id={0} -pa={1}", Username, pPassword);

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

        #region Properties

        /// <summary>
        /// Returns the version of this dsmadmc exeutable.
        /// (Currently only implemented on windows...)
        /// </summary>
        /// <value>The dsm admc version.</value>
        public Version DsmAdmcVersion
        {
            get
            {
                return clsTSMPlatform.sThisPlatform.DsmAdmcVersion; 
            }
        }

        /// <summary>
        /// Gets or sets the DSM_LOG environment variable.
        /// </summary>
        /// <remarks>
        /// The errorlogname and schedlogname options override DSM_LOG. If you specify the errorlogname client option, the file is stored in the directory specified by the errorlogname option and not in the location specified by DSM_LOG. If you specify the schedlogname client option, it is written to the directory specified by the schedlogname option and not in the location specified by DSM_LOG.
        /// The log files cannot be symbolic links.Tivoli Storage Manager detects any such links, delete the links, then exit the operation.This prevents Tivoli Storage Manager from overwriting protected data.The affected logs are created as files in a subsequent operation.
        /// </remarks>
        public string DsmLog
        {
            get
            {
                return pEnvironments["DSM_LOG"];
            }
            set
            {
                pEnvironments["DSM_LOG"] = value;
            }
        }

        #endregion
    }
}
