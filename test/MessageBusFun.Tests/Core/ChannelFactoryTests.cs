using MessageBusFun.Core;
using Moq;
using NUnit.Framework;

namespace MessageBusFun.Tests.Core
{
    [TestFixture]
    public class ChannelFactoryTests
    {
        [Test]
        public void CreateWithProvider_GivenProviderAndChannelName_WillReturnNewChannel()
        {
            var channelFactory = new ChannelFactory();

            var channel = channelFactory.CreateWithProvider("name", Mock.Of<IProvider>());

            Assert.That(channel, Is.InstanceOf<IChannel>());
        }
    }
}
