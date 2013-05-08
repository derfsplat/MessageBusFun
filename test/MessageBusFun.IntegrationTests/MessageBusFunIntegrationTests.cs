using MessageBusFun.Core;
using Moq;
using NUnit.Framework;

namespace MessageBusFun.IntegrationTests
{
    [TestFixture]
    public class MessageBusFunIntegrationTests
    {
        private IMessageBus _messageBus;

        [SetUp]
        public void SetUp()
        {
            _messageBus = new MessageBusFactory().Create();
        }

        [Test]
        public void SimplePublishTest()
        {
            var message = Mock.Of<IMessage>();
            var provider = Mock.Of<IProvider>(x => x.Message == message);

            const string channelName = "test";

            _messageBus.RegisterProvider(provider: provider, channelName: channelName);

            Assert.That(_messageBus.Channels, Is.EquivalentTo(new[]{ channelName }));

            var subscriberMock = new Mock<ISubscriber>();

            _messageBus.RegisterSubscriber(subscriber: subscriberMock.Object, channelName: channelName);

            _messageBus.Publish(provider);

            subscriberMock.Verify(x => x.HandleMessage(message));
        }

        [Test]
        public void UnsubscribeLastProviderTest()
        {
            var message = Mock.Of<IMessage>();
            var provider = Mock.Of<IProvider>();

            const string channelName = "test";

            _messageBus.RegisterProvider(provider: provider, channelName: channelName);

            Assert.That(_messageBus.Channels, Is.EquivalentTo(new[]{ channelName }));

            var subscriberMock = new Mock<ISubscriber>();

            _messageBus.RegisterSubscriber(subscriber: subscriberMock.Object, channelName: channelName);

            _messageBus.DeregisterProvider(provider);

            subscriberMock.Verify(x => x.HandleChannelUnavailable(channelName));
        }
    }
}
