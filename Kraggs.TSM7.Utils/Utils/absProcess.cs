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

using System.Collections.Specialized;

namespace Kraggs.TSM7.Utils
{
    public abstract class absProcess
    {
        private const int CONST_MINIMUM_TIMEOUTMS = 30000; // minimum 30 sec timeout. !DEFAULT IS NO TIMEOUT!

        protected string pExecutable;
        protected string pWorkingDir;

        //protected List<KeyValuePair<string, string>> pEnvironments;
        //protected StringDictionary pEnvironments;
        protected SortedDictionary<string, string> pEnvironments;

        private bool flagValidated;

        /// <summary>
        /// Enables/disables saving the last dsmadmc command run in 'DebugLastCommand'
        /// </summary>
        public bool DebugSaveLastCommand { get; set; } // = false;
        /// <summary>
        /// If enabled, saves the last dsmadmc command run here for use during debugging.
        /// </summary>
        public string DebugLastCommand { get; private set; }        

        /// <summary>
        /// protected empty constructor. Implementer is required to set pExecutable and call Validate before runprocess.
        /// </summary>
        protected absProcess()
        {
            // dummy ctor.
            //pEnvironments = new List<KeyValuePair<string, string>>();
            pEnvironments = new SortedDictionary<string, string>();
        }

        protected absProcess(string Executable, string WorkingDir = null)
        {
            this.pExecutable = Executable;
            this.pWorkingDir = WorkingDir;

            this.Validate();
        }

        /// <summary>
        /// Validates pExecutable, and optionally sets working dir to path of executable.
        /// </summary>
        protected void Validate()
        {
            //Debug.Assert(pExecutable != null && pExecutable.Length > 0, $"pExecutable is not valid '{pExecutable}'");
            //Debug.Assert(pWorkingDir != null && pWorkingDir.Length > 0, $"pExecutable is not valid '{pWorkingDir}'");

            if (string.IsNullOrWhiteSpace(this.pExecutable))
                throw new ArgumentNullException("pExecutable");
            else
            {
                if (!File.Exists(this.pExecutable))
                    throw new FileNotFoundException(this.pExecutable);
            }

            if(string.IsNullOrWhiteSpace(pWorkingDir))
            {
                // get workingdir from executable.
                if (Path.IsPathRooted(this.pExecutable))
                {
                    this.pWorkingDir = Path.GetDirectoryName(this.pExecutable);
                }
                else
                    throw new NotImplementedException();
            }

            this.flagValidated = true;
        }

        ///<summary>
        ///Sets up a process for running with specified argument.
        ///</summary>
        ///<param name="arguments">Arguments to call executable with.</param>
        ///<param name="output">optional list of output from run.</param>
        ///<param name="error">optional list of error from run.</param>
        ///<param name="TimeoutMs">Optional timeout in ms for timing out a process. NOTE: throws timedoutexception.</param>
        ///<returns>Exit code of process.</returns>
        protected virtual int RunCommand(string arguments, List<string> output = null, List<string> error = null, int TimeoutMs = 0)
        {
            var proc = new Process();
            proc.StartInfo = GenerateProcessStartInfo(arguments);

            if(output != null)
            {
                proc.OutputDataReceived += delegate(object sender, DataReceivedEventArgs e)
                {
                    if (e.Data != null)
                        output.Add(e.Data);
                };

                proc.StartInfo.RedirectStandardOutput = true;
            }

            if(error != null)
            {
                proc.ErrorDataReceived += delegate(object sender, DataReceivedEventArgs e)
                {
                    if(e.Data != null)
                        error.Add(e.Data);
                };

                proc.StartInfo.RedirectStandardError = true;
            }

            return RunProcess(proc, TimeoutMs);
        }

        ///// <summary>
        ///// Sets up a process for running with specified argument.
        ///// </summary>
        ///// <param name="arguments">Arguments to call executable with.</param>
        ///// <param name="output">optional list of output from run.</param>
        ///// <param name="error">optional list of error from run.</param>
        ///// <param name="TimeoutMs">Optional timeout in ms for timing out a process. NOTE: throws timedoutexception.</param>
        ///// <returns>Exit code of process.</returns>
        //protected virtual int RunCommand(string arguments, List<string> output = null, List<string> error = null, int TimeoutMs = 0)
        //{
        //    DataReceivedEventHandler deloutput = null;
        //    DataReceivedEventHandler delerror = null;

        //    if (output != null)
        //        deloutput = delegate(object sender, DataReceivedEventArgs e)
        //        {
        //            if (e.Data != null)
        //                output.Add(e.Data);
        //        };

        //    if (error != null)
        //        delerror = delegate(object sender, DataReceivedEventArgs e)
        //        {
        //            if (e.Data != null)
        //                error.Add(e.Data);
        //        };

        //    return RunCommand(arguments, deloutput, delerror, TimeoutMs);
        //}

        /// <summary>
        /// Sets up a process for running with specified arguments and optionally custom callbacks.
        /// </summary>
        /// <param name="arguments">Process arguments.</param>
        /// <param name="output">output callback</param>
        /// <param name="error">error callback</param>
        /// <param name="TimeoutMs">Optional timeout in ms for timing out a process. NOTE: throws timedoutexception.</param>
        /// <returns></returns>
        protected virtual int RunCommand(string arguments, DataReceivedEventHandler output = null, DataReceivedEventHandler error = null, int TimeoutMs = 0)
        {
            var proc = new Process();
            proc.StartInfo = GenerateProcessStartInfo(arguments);

            if(output != null)
            {
                proc.OutputDataReceived += output;
                proc.StartInfo.RedirectStandardOutput = true;
            }

            if(error != null)
            {
                proc.ErrorDataReceived += error;
                proc.StartInfo.RedirectStandardError = true;
            }

            return RunProcess(proc, TimeoutMs);
        }

        /// <summary>
        /// Runs the preconfigured process object.
        /// Process is always disposed on return!.
        /// </summary>
        /// <param name="proc"></param>
        /// <param name="TimeoutMs">Optional timeout in ms for timing out a process. NOTE: throws timedoutexception.</param>
        /// <returns>Exit code of process.</returns>
        protected int RunProcess(Process proc, int TimeoutMs = 0)
        {
            // to ensure executable and working dir is set befora call, this.Validate() should be called in a ctor.
            if (!flagValidated)
                throw new Exception("Programming error: absProcess::Validate is not called.");

            if (DebugSaveLastCommand)
                this.DebugLastCommand = string.Format(
                    "Exe:'{0}', Args:'{1}'", proc.StartInfo.FileName, proc.StartInfo.Arguments);

            //if(pEnvironments != null)
            //{
            //    foreach(var kv in pEnvironments)
            //    {
            //        proc.StartInfo.EnvironmentVariables.Add
            //    }
            //}

            using(proc)
            {
                if (proc.Start())
                {
                    if (proc.StartInfo.RedirectStandardOutput)
                        proc.BeginOutputReadLine();
                    if (proc.StartInfo.RedirectStandardError)
                        proc.BeginErrorReadLine();

                    if (TimeoutMs > CONST_MINIMUM_TIMEOUTMS)
                    {
                        bool flagTimedOut = proc.WaitForExit(TimeoutMs);

                        if (!flagTimedOut)
                            proc.WaitForExit(); // wait for async redirect handlers to complete.
                        else
                            throw new TimeoutException(string.Format("Timedout waiting for external process '{0}' to finish.", proc.StartInfo.FileName));
                    }
                    else
                        proc.WaitForExit(); // wait forever!
                }
                else
                    throw new Exception(string.Format("Failed to start external process: '{0}'", proc.StartInfo.FileName));

                return proc.ExitCode;
            }

            // using auto disposes proc object.
        }

        /// <summary>
        /// Generates the standard process start info.
        /// </summary>
        /// <param name="Arguments">Argument to use.</param>
        /// <returns></returns>
        protected virtual ProcessStartInfo GenerateProcessStartInfo(string Arguments)
        {
            var psi = new ProcessStartInfo()
            {
                UseShellExecute = false,
                CreateNoWindow = true,
                //WindowStyle = ProcessWindowStyle.Hidden,
                Arguments = Arguments,
                WorkingDirectory = this.pWorkingDir,
                FileName = this.pExecutable,                
            };

            if(pEnvironments != null)
            {
                foreach (var kv in pEnvironments)
                    //psi.EnvironmentVariables.Add(kv.Key, kv.Value);
                    psi.Environment.Add(kv);
            }

            return psi;
        }
    }
}
