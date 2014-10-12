using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Kraggs.TSM7.Utils;

namespace Kraggs.TSM7.Utils.Tests.Bugs
{
    [TestFixture]
    public class clsDsmAdmcBugs
    {
        [Test]
        public void BugOutputIsNullDisablesDetailedException()
        {
            //var admc = new clsDsmAdmc(pUsername, pPassword, pServer, pPort);
            var admc = TestUtils.CreateDsmAdmc();

            var ex = Assert.Throws<Exception>(delegate
            {
                var exitCode = admc.RunTSMCommandToList("An error will occur");
            });

            Assert.That(ex.Message != "An unspecified error occured. Check with dsmerror.log for more info.", "Generic non descriptive error message given.");
        }

    }
}
