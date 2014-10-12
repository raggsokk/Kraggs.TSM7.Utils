using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using NUnit.Framework;

using Kraggs.TSM7.Utils;

namespace Kraggs.TSM7.Utils.Tests.Coverage
{
    /// <summary>
    /// This is a test fixture for testing actual tsm server handling.
    /// This should include:
    ///     admin user is locked.
    ///     Access denied.
    ///     Password for admin user is expired. (if possible)
    ///     ...
    ///     
    /// This requires setting up test state on an actual tsm server.
    /// Therefore a FixtureSetup runs a macro setting this up.
    /// To Correctly run this macro, create a file nunit.tsmpass with content:
    ///     user%pass@server
    ///     
    /// For TSM Server Connection handling see clsTSMServerConnectionCoverage.
    /// </summary>
    //[TestFixture()]
    public class clsTSMServerErrorCoverage
    {
        // OLD CODE. NEEDS TO BE CLEANED UP!


        //private static bool flagTSMServerTestState = false;

        //private System.Net.NetworkCredential tsmcred;

        //[TestFixtureSetUp]
        //public void SetupTSMServerTestState()
        //{
        //    if (flagTSMServerTestState) 
        //        Assert.Fail("Fixture setup called multiple times.");
        //    flagTSMServerTestState = true;

        //    // rest of setup.

        //    // 1. read in user, pass and server.
            
        //    var line = File.ReadAllText(string.Format("..{0}..{0}nunit.tsmpass", Path.DirectorySeparatorChar));
        //    var prosent = line.IndexOf('%');
        //    var alpha = line.LastIndexOf('@');

        //    var user = line.Substring(0, prosent);
        //    var server = line.Substring(alpha + 1);
        //    var pass = line.Substring(prosent + 1, alpha - prosent - 1);

        //    this.tsmcred = new System.Net.NetworkCredential(user, pass, server);

        //    //var dsmadmc = new clsDsmAdmc()
        //    throw new NotImplementedException(string.Format(
        //        "'dsmadmc not implemented' -tcps={0} -id={1} -pa={2}", server, user, pass));
        //}

        //[TestFixtureTearDown]
        //public void CleanupTSMServerTestState()
        //{ 
        //    // 
        //    if (!flagTSMServerTestState)
        //        Assert.Fail("Fixture cleanup called but setup was not called??");




        //    //flagTSMServerTestState = false;
        //}
    }
}
