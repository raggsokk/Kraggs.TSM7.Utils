using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

using NUnit.Framework;

using Kraggs.TSM7.Utils;

using Kraggs.TSM7.Utils.Windows;

namespace Kraggs.TSM7.Utils.Tests.UseCases
{
    [TestFixture()]
    public class clsTSMRegistryUseCases
    {
        [Test]
        public void SimpleBAClientUseCase()
        {
            var prod = clsTSMRegistry.GetProductByName("TSM Administrative Client");

            var binDsmAdmc = (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX) ? "dsmadmc" : "dsmadmc.exe";

            Assert.IsTrue(File.Exists(Path.Combine(
                prod.Path, binDsmAdmc)));            
        }
    }
}
