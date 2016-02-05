using Cloud.Common.Contracts;
using Cloud.Common.Core;
using Cloud.Common.Extensions;
using Cloud.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Cloud.Common.UnitTests
{
    public class CommonUnitTests: BaseUnitTest
    {
        public CommonUnitTests()
            :base()
        {

        }

        [Fact]
        public void FirstUnitTestAsTrue()
        {
            Assert.True(true);
        }

        [Fact]
        public void XmlCreationTest()
        {
            var messageFactory = ServiceProvider.GetService<IClientMessageFactory>();
            
            var encodedObject = messageFactory.CreateMessage(new MessageDefinitions{ Owner = "Server1" });
            Assert.NotNull(encodedObject);

            var decodeObject = messageFactory.GetMessage<MessageDefinitions>(encodedObject.FromUtf8Bytes());
            Assert.Equal(decodeObject.Owner, "Server1");
        }
    }
}
