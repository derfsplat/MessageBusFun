using MessageBusFun.Core;
using Moq;
using NUnit.Framework;

namespace MessageBusFun.Tests.Core
{
    [TestFixture]
    public class MessageBusTests
    {
        private IMessageBus _messageBus;

        [SetUp]
        public void SetUp()
        {
            _messageBus = new MessageBus();
        }

        [Test]
        public void RegisterProvider_GivenProviderAndChannel_WillAddToCollection()
        {
            var provider = Mock.Of<IProvider>();
            const string channelName = "test";

            var channelsMock = new Mock<IChannelCollection>();
            var messageBus = new MessageBus(channelsMock.Object);

            messageBus.RegisterProvider(channelName: channelName, provider: provider);
            
            channelsMock.Verify(x => x.AddProvider(channelName, provider));
        }

        [Test]
        public void Channels_GivenNoProviders_WillReturnEmptyCollection()
        {
            var channels = _messageBus.Channels;
        
            Assert.That(channels, Is.Empty);
        }

        [Test]
        public void Channels_GivenSubscribedProvider_WillReturnProviderChannelInCollection()
        {
            var provider = Mock.Of<IProvider>();
            const string channelName = "test";

            _messageBus.RegisterProvider(channelName: channelName, provider: provider);

            var channels = _messageBus.Channels;

            Assert.That(channels, Has.Count.EqualTo(1));
            Assert.That(channels, Has.Exactly(1).EqualTo(channelName));
        }

        // TODO
        //[Test]
        //public void Channels_GivenUnregisteredProvider_WillNotReturnProviderChannel()
        //{
           
        //}

        [Test]
        public void Channels_GivenMultipleChannels_WillReturnAllProvidersChannels()
        {
            var provider1 = Mock.Of<IProvider>();
            var provider2 = Mock.Of<IProvider>();
            const string channelName1 = "test1";
            const string channelName2 = "test2";

            _messageBus.RegisterProvider(channelName: channelName1, provider: provider1);
            _messageBus.RegisterProvider(channelName: channelName2, provider: provider2);

            var channels = _messageBus.Channels;

            Assert.That(channels, Has.Count.EqualTo(2));
            Assert.That(channels, Has.Exactly(1).EqualTo(channelName1));
            Assert.That(channels, Has.Exactly(1).EqualTo(channelName2));
        }

        [Test]
        public void Channels_GivenMultipleProvidersForSameChannel_WillOnlyReturnSingleInstanceOfChannel()
        {
            var provider1 = Mock.Of<IProvider>();
            var provider2 = Mock.Of<IProvider>();
            const string channelName = "test";

            _messageBus.RegisterProvider(channelName: channelName, provider: provider1);
            _messageBus.RegisterProvider(channelName: channelName, provider: provider2);

            var channels = _messageBus.Channels;

            Assert.That(channels, Has.Count.EqualTo(1));
            Assert.That(channels, Has.Exactly(1).EqualTo(channelName));
        }
    }
}
