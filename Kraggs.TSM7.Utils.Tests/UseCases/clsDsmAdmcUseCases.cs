using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

using NUnit.Framework;

using Kraggs.TSM7.Utils;

namespace Kraggs.TSM7.Utils.Tests.UseCases
{
    [TestFixture()]
    public class clsDsmAdmcUseCases
    {
        [Test]
        public void DsmAdmcCommandToList()
        {
            var admc = TestUtils.CreateDsmAdmc();

            var list = new List<string>();

            var exitCode = admc.RunTSMCommandToList("q ses", list);

            Assert.AreEqual((int)AdmcExitCode.Ok, (int)exitCode, "Command 'q ses' failed to return ok.");

            Assert.NotNull(list);

            Assert.True(list.Count > 0);
        }
    }
}
