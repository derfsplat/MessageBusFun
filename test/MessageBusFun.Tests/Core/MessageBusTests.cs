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

        [Test]
        public void DeregisterProvider_GivenProviderAssociatedWithChannel_WillDeregisterFromChannel()
        {
            var provider = Mock.Of<IProvider>();
            const string channelName = "test";

            var channelMock = new Mock<IChannel>();

            _channelsMock.Setup(x => x.ChannelForProvider(provider)).Returns(channelMock.Object);

            _messageBus.DeregisterProvider(provider: provider);

            channelMock.Verify(x => x.DeregisterProvider(provider));
        }

        [Test]
        public void DeregisterProvider_GivenProviderNotAssociatedWithChannel_WillNotAct()
        {
            var existingProvider = Mock.Of<IProvider>();
            var newProvider = Mock.Of<IProvider>();
            const string channelName = "test";

            var channelMock = new Mock<IChannel>();

            _channelsMock.Setup(x => x.ChannelForProvider(existingProvider)).Returns(channelMock.Object);

            _messageBus.DeregisterProvider(provider: newProvider);

            channelMock.Verify(x => x.DeregisterProvider(newProvider), Times.Never());
        }

        [Test]
        public void RegisterSubscriber_GivenNameAssociatedWithExistingChannel_WillRegisterSubscriber()
        {
            const string channelName = "test";
            var channelMock = new Mock<IChannel>();

            _channelsMock.Setup(x => x.ChannelWithName(channelName)).Returns(channelMock.Object);

            var subscriber = Mock.Of<ISubscriber>();
        
            _messageBus.RegisterSubscriber(subscriber: subscriber, channelName: channelName);

            channelMock.Verify(x => x.RegisterSubscriber(subscriber));
        }

        [Test]
        public void RegisterSubscriber_GivenNameNotAssociatedWithExistingChannel_WillNotRegisterSubscriber()
        {
            const string channelNameExisting = "test";
            const string channelNameNew = "new";

            var channelMock = new Mock<IChannel>();

            _channelsMock.Setup(x => x.ChannelWithName(channelNameExisting)).Returns(channelMock.Object);

            var subscriber = Mock.Of<ISubscriber>();

            _messageBus.RegisterSubscriber(subscriber: subscriber, channelName: channelNameNew);

            channelMock.Verify(x => x.RegisterSubscriber(subscriber), Times.Never());
        }

        [Test]
        public void DeregisterSubscriber_GivenNameAssociatedWithExistingChannel_WillDeregisterSubscriber()
        {
            const string channelName = "test";
            var channelMock = new Mock<IChannel>();

            _channelsMock.Setup(x => x.ChannelWithName(channelName)).Returns(channelMock.Object);

            var subscriber = Mock.Of<ISubscriber>();
        
            _messageBus.DeregisterSubscriber(subscriber: subscriber, channelName: channelName);

            channelMock.Verify(x => x.DeregisterSubscriber(subscriber));
        }

        [Test]
        public void Publish_GivenProviderWithAssociatedChannel_WillSendMessageToChannel()
        {
            var provider = Mock.Of<IProvider>();
            var channelMock = new Mock<IChannel>();
            _channelsMock.Setup(x => x.ChannelForProvider(provider)).Returns(channelMock.Object);

            _messageBus.Publish(provider);

            channelMock.Verify(x => x.SendMessage(provider));
        }

        [Test]
        public void Publish_GivenProviderWithNoAssociatedChannel_WillNotSendMessage()
        {
            var providerMock = new Mock<IProvider>();

            _messageBus.Publish(providerMock.Object);

            providerMock.Verify(x => x.Message, Times.Never());
        }
    }
}
