using System;
using System.Collections.Generic;
using System.Linq;
using MessageBusFun.Core;
using Moq;
using NUnit.Framework;

namespace MessageBusFun.Tests.Core
{
    [TestFixture]
    public class ChannelTests
    {
        private IChannel _channel;
        private HashSet<IProvider> _providers;
        private HashSet<ISubscriber> _subscribers;
        private string _channelName;

        [SetUp]
        public void SetUp()
        {
            _providers = new HashSet<IProvider>();
            _subscribers = new HashSet<ISubscriber>();
            _channelName = "name";
            _channel = new Channel(name: _channelName,
                                   provider: Mock.Of<IProvider>(),
                                   providers: _providers,
                                   subscribers: _subscribers);
        }

        [Test]
        public void RegisterProvider_WillAddProvider()
        {
            var provider = Mock.Of<IProvider>();

            _channel.RegisterProvider(provider);

            Assert.That(_providers, Contains.Item(provider));
        }

        [Test]
        public void RegisterProvider_WhenCalledMultipleTimesWithSameProvider_WillAddProvider()
        {
            var provider = Mock.Of<IProvider>();

            _channel.RegisterProvider(provider);
            _channel.RegisterProvider(provider);

            Assert.That(_providers, Has.Exactly(1).EqualTo(provider));
        }

        [Test]
        public void DeregisterProvider_WillRemoveProvider()
        {
            var provider = Mock.Of<IProvider>();
            _providers.Add(provider);

            _channel.DeregisterProvider(provider);

            Assert.That(_providers, Has.None.EqualTo(provider));
        }

        [Test]
        public void DeregisterProvider_WhenCalledMultipleTimesWithSameProvider_WillRemoveProvider()
        {
            var provider = Mock.Of<IProvider>();
            _providers.Add(provider);

            _channel.DeregisterProvider(provider);
            _channel.DeregisterProvider(provider);

            Assert.That(_providers, Has.None.EqualTo(provider));
        }

        [Test]
        public void DeregisterProvider_WhenRemovingTheLastProvider_WillCallChannelUnavailableHandlerForSubscribers()
        {
            var provider = _providers.Single();

            var subscriberMocks = new[]
                {
                    new Mock<ISubscriber>(),
                    new Mock<ISubscriber>()
                };

            Array.ForEach(subscriberMocks, s => _subscribers.Add(s.Object));

            _channel.DeregisterProvider(provider);

            Array.ForEach(subscriberMocks, s => s.Verify(x => x.HandleChannelUnavailable(_channelName)));
        }

        [Test]
        public void RegisterSubscriber_WillAddSubscriber()
        {
            var subscriber = Mock.Of<ISubscriber>();

            _channel.RegisterSubscriber(subscriber);

            Assert.That(_subscribers, Contains.Item(subscriber));
        }

        [Test]
        public void RegisterSubscriber_WhenCalledMultipleTimesWithSameSubscriber_WillAddSubscriber()
        {
            var subscriber = Mock.Of<ISubscriber>();

            _channel.RegisterSubscriber(subscriber);
            _channel.RegisterSubscriber(subscriber);

            Assert.That(_subscribers, Has.Exactly(1).EqualTo(subscriber));
        }

        [Test]
        public void DeregisterSubscriber_WillRemoveSubscriber()
        {
            var subscriber = Mock.Of<ISubscriber>();
            _subscribers.Add(subscriber);

            _channel.DeregisterSubscriber(subscriber);

            Assert.That(_subscribers, Has.None.EqualTo(subscriber));
        }

        [Test]
        public void DeregisterSubscriber_WhenCalledMultipleTimesWithSameSubscriber_WillRemoveSubscriber()
        {
            var subscriber = Mock.Of<ISubscriber>();
            _subscribers.Add(subscriber);

            _channel.DeregisterSubscriber(subscriber);
            _channel.DeregisterSubscriber(subscriber);

            Assert.That(_subscribers, Has.None.EqualTo(subscriber));
        }

        [Test]
        public void SendMessage_GivenRegisteredProvider_WillSendMessageToSubscribers()
        {
            var message = Mock.Of<IMessage>();
            var provider = Mock.Of<IProvider>(x => x.Message == message);
            _providers.Add(provider);

            var subscriberMocks = new[]
                {
                    new Mock<ISubscriber>(),
                    new Mock<ISubscriber>()
                };

            Array.ForEach(subscriberMocks, s => _subscribers.Add(s.Object));

            _channel.SendMessage(provider);

            Array.ForEach(subscriberMocks, s => s.Verify(x => x.HandleMessage(message)));
        }

        [Test]
        public void SendMessage_GivenNonRegisteredProvider_WillFailSilently()
        {
            var message = Mock.Of<IMessage>();
            var provider = Mock.Of<IProvider>(x => x.Message == message);

            var subscriberMocks = new[]
                {
                    new Mock<ISubscriber>(),
                    new Mock<ISubscriber>()
                };

            Array.ForEach(subscriberMocks, s => _subscribers.Add(s.Object));

            _channel.SendMessage(provider);

            Array.ForEach(subscriberMocks, s => s.Verify(x => x.HandleMessage(message), times: Times.Never()));
        }

        [Test]
        public void IsProviderRegistered_GivenRegisteredProvider_ReturnsTrue()
        {
            var provider = Mock.Of<IProvider>();

            _providers.Add(provider);

            Assert.That(_channel.IsProviderRegistered(provider), Is.EqualTo(true));
        }

        [Test]
        public void IsProviderRegistered_GivenNonRegisteredProvider_ReturnsFalse()
        {
            var provider = Mock.Of<IProvider>();

            Assert.That(_channel.IsProviderRegistered(provider), Is.EqualTo(false));
        }

        [Test]
        public void HasProviders_GivenRegisteredProvider_ReturnsTrue()
        {
            _providers.Add(Mock.Of<IProvider>());

            Assert.That(_channel.HasProviders, Is.EqualTo(true));
        }

        [Test]
        public void HasProviders_GivenNoRegisteredProviders_ReturnsFalse()
        {
            _providers.Clear();

            Assert.That(_channel.HasProviders, Is.EqualTo(false));
        }
    }
}
