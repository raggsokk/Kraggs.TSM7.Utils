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
    public class clsTSMPlatformUseCases
    {
        [Test]
        public void SimpleTSMPlatformUseCase()
        {
            var platform = clsTSMPlatform.TSMPlatform;

            Assert.IsNotNull(platform);

            Assert.IsTrue(Directory.Exists(platform.BAClientPath));
            Assert.IsTrue(File.Exists(platform.DsmAdmcBinary));
        }
    }
}
