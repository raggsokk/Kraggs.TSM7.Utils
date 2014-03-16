using System;
using System.Collections.Generic;
using System.Text;

using System.IO;

using NUnit.Framework;

using Kraggs.TSM7.Utils;

using Kraggs.TSM7.Utils.Windows;
using Kraggs.TSM7.Utils.Linux;

namespace Kraggs.TSM7.Utils.Tests.Coverage
{
    [TestFixture()]
    public class clsTSMPlatformCoverage
    {
        [Test]
        public void PlatformCoverage()
        {
            var platform = clsTSMPlatform.TSMPlatform;
            Assert.IsNotNull(platform);

            var type = platform.GetType();
            if (Environment.OSVersion.Platform == PlatformID.Unix)
                Assert.IsTrue(type == typeof(clsLinuxPlatform));
            else
                Assert.IsTrue(type == typeof(clsWinPlatform));
        }

        [Test]
        public void PlatformNotInstalledWindowsCoverage()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix ||
                Environment.OSVersion.Platform == PlatformID.MacOSX)
                Assert.Ignore();

            // existing but wrong key structure.
            clsTSMRegistry.Initialize(@"SOFTWARE\IBM\kljsdlfj");
            
            var winplatform = new clsWinPlatform();

            Assert.IsNullOrEmpty(winplatform.BAClientPath); 
            Assert.IsNullOrEmpty(winplatform.DsmcBinary);
            Assert.IsNullOrEmpty(winplatform.DsmAdmcBinary);
            Assert.IsNullOrEmpty(winplatform.DsmOpt);
            Assert.IsNull(winplatform.DsmAdmcVersion);
            
            //var platform = clsTSMPlatform.TSMPlatform;

            // reread correct values for other cases.
            clsTSMRegistry.Initialize();
        }

        [Test]
        public void BAClientPathCoverage()
        {
            var t = clsTSMPlatform.TSMPlatform.BAClientPath;
            Assert.IsNotNullOrEmpty(t);
            Assert.IsTrue(Directory.Exists(t));
        }

        [Test]
        public void DsmcBinaryCoverage()
        {
            var t = clsTSMPlatform.TSMPlatform.DsmcBinary;
            Assert.IsNotNullOrEmpty(t);
            Assert.IsTrue(File.Exists(t));
        }

        [Test]
        public void DsmAdmcBinaryCoverage()
        {
            var t = clsTSMPlatform.TSMPlatform.DsmAdmcBinary;
            Assert.IsNotNullOrEmpty(t);
            Assert.IsTrue(File.Exists(t));
        }

        [Test]
        public void DsmOptCoverage()
        {
            var t = clsTSMPlatform.TSMPlatform.DsmOpt;
            Assert.IsNotNullOrEmpty(t);
            //Assert.IsTrue(File.Exists(t));
        }

    }
}
