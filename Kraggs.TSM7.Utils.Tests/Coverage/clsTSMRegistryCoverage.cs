using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Kraggs.TSM7.Utils;
using Kraggs.TSM7.Utils.Windows;

namespace Kraggs.TSM7.Utils.Tests.Coverage
{
    [TestFixture()]
    public class clsTSMRegistryCoverage
    {
        [Test]
        public void SimpleBAClientProductNotNullUseCase()
        {
            if(!clsTSMPlatform.IsWindows)
                Assert.Ignore();

            var prod = clsTSMRegistry.GetProductByName("TSM Administrative Client");

            Assert.NotNull(prod);            
        }

        [Test]
        public void SimpleBAClientRequiredLevelTest()
        {
            if(!clsTSMPlatform.IsWindows)
                Assert.Ignore();

            var prod = clsTSMRegistry.GetProductByName("TSM Administrative Client");
            // check tsm 7.1. (it works with lower level too, but this library targets that level.
            Assert.GreaterOrEqual(7, prod.PtfLevel.Major);
            Assert.GreaterOrEqual(1, prod.PtfLevel.Minor);

        }

        [Test]
        public void SimpleBAClientDirectoryExistTest()
        {
            if(!clsTSMPlatform.IsWindows)
                Assert.Ignore();

            var prod = clsTSMRegistry.GetProductByName("TSM Administrative Client");
            
            Assert.IsTrue(System.IO.Directory.Exists(prod.Path));
        }

        [Test]
        public void ValidateNotInstalledRegistryHandling()
        {
            if(!clsTSMPlatform.IsWindows)
                Assert.Ignore();

            clsTSMRegistry.Initialize("sdlfkjsdlfj");

            var prod = clsTSMRegistry.GetProductByName("TSM Administrative Client");

            Assert.IsNull(prod);

            // reread with correct values for other tests.
            clsTSMRegistry.Initialize();
        }

        [Test]
        public void ValidateNonExistingProductHandling()
        {
            if (!clsTSMPlatform.IsWindows)
                Assert.Ignore();

            var prod = clsTSMRegistry.GetProductByName("slkjdflsjkdf");
            Assert.IsNull(prod);
        }
    }
}
