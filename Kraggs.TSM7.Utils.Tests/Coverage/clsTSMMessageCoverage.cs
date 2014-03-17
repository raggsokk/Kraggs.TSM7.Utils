using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using Kraggs.TSM7.Utils;

namespace Kraggs.TSM7.Utils.Tests.Coverage
{
    [TestFixture]
    public class clsTSMMessageCoverage
    {
        [Test]
        public void ParsePrefixTest()
        {
            var test = "ANR2000E Unknown command - QUERY SC";

            clsTSMMessage msg;

            if (clsTSMMessage.TryParse(test, out msg))
            {
                StringAssert.AreEqualIgnoringCase("ANR", msg.Prefix);
            }
            else
                Assert.Fail("Failed to parse TSMMessage");
        }

        [Test]
        public void ParseNumberTest()
        {
            var test = "ANR2000E Unknown command - QUERY SC";

            clsTSMMessage msg;

            if (clsTSMMessage.TryParse(test, out msg))
            {                
                Assert.AreEqual(2000, msg.Number);             
            }
            else
                Assert.Fail("Failed to parse TSMMessage");
        }

        [Test]
        public void ParseTypeTest()
        {
            var test = "ANR2000E Unknown command - QUERY SC";

            clsTSMMessage msg;

            if (clsTSMMessage.TryParse(test, out msg))
            {
                Assert.AreEqual('E', msg.Type);   
            }
            else
                Assert.Fail("Failed to parse TSMMessage");
        }

        [Test]
        public void ParseReassembleTest()
        {
            var test = "ANR2000E Unknown command - QUERY SC";

            clsTSMMessage msg;

            if (clsTSMMessage.TryParse(test, out msg))
            {
                var fullmsg = msg.FullMessage;
                StringAssert.AreEqualIgnoringCase(test, fullmsg);
           
            }
            else
                Assert.Fail("Failed to parse TSMMessage");            
        }
    }
}
