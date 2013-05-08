using MessageBusFun.Core;
using Moq;
using NUnit.Framework;

namespace MessageBusFun.Tests.Core
{
    [TestFixture]
    public class MessageBusTests
    {
        private IMessageBus _messageBus;
        private Mock<IChannelCollection> _channelsMock;

        [SetUp]
        public void SetUp()
        {
            _channelsMock = new Mock<IChannelCollection>();
            _messageBus = new MessageBus(_channelsMock.Object);
        }

        [Test]
        public void RegisterProvider_GivenProviderAndChannel_WillAddToCollection()
        {
            var provider = Mock.Of<IProvider>();
            const string channelName = "test";

            _messageBus.RegisterProvider(channelName: channelName, provider: provider);
            
            _channelsMock.Verify(x => x.AddProvider(channelName, provider));
        }
    }
}
