using NUnit.Framework;
using Does = NUnit.Framework.Is;

namespace MessageBusFun.Core.Tests
{
    [TestFixture]
    public class SendMessageViaChannelTest
    {
        private MessageBus _messageBus;
        private const string Channel = "MyChannel";
        private const string NewChannel = "NewChannel";

        [SetUp]
        public void SetUp()
        {
            _messageBus = new MessageBus();
        }

        [Test]
        public void GivenRegisteredProviderSendingMessage_RegisteredSubscriberShouldReceiveSameMessage()
        {
            var provider = new Provider();
            _messageBus.Register(Channel, provider);

            var subscriber = new Subscriber();
            _messageBus.Register(Channel, subscriber);

            var message = new Message();
            _messageBus.MessageFrom(provider, message);

            Assert.That(subscriber.ReceivedMessages, Contains.Item(message));
        }

        [Test]
        public void GivenRegisteredProviderSendingMessage_RegisteredSubscriber_ForDifferentChannel_ShouldNotReceiveMessage()
        {
            var provider = new Provider();
            _messageBus.Register(Channel, provider);

            var subscriber = new Subscriber();
            _messageBus.Register(NewChannel, subscriber);

            var message = new Message();
            _messageBus.MessageFrom(provider, message);

            Assert.That(subscriber.ReceivedMessages, Does.Not.Contains(message));
        }

        [Test]
        public void GivenRegisteredProviderSendingMessage_RegisteredMultipleSubscribers_ForDifferentChannels_ReceiveCorrectMessages()
        {
            var provider = new Provider();
            _messageBus.Register(Channel, provider);

            var subscriber = new Subscriber();
            _messageBus.Register(NewChannel, subscriber);
            var newSubscriber = new Subscriber();
            _messageBus.Register(Channel, newSubscriber);

            var message = new Message();
            _messageBus.MessageFrom(provider, message);

            Assert.That(subscriber.ReceivedMessages, Does.Not.Contains(message));
            Assert.That(newSubscriber.ReceivedMessages, Contains.Item(message));
        }

        [Test]
        public void GivenRegisteredProviderSendingMessage_RegisteredMultipleSubscribers_ForDifferentChannels_MultipleSubscribersReceiveCorrectMessages()
        {
            var provider = new Provider();
            _messageBus.Register(Channel, provider);

            var subscriber = new Subscriber();
            _messageBus.Register(NewChannel, subscriber);
            var newSubscriber = new Subscriber();
            _messageBus.Register(Channel, newSubscriber);
            var secondNewSubscriber = new Subscriber();
            _messageBus.Register(Channel, secondNewSubscriber);

            var message = new Message();
            _messageBus.MessageFrom(provider, message);

            Assert.That(subscriber.ReceivedMessages, Does.Not.Contains(message));
            Assert.That(newSubscriber.ReceivedMessages, Contains.Item(message));
            Assert.That(secondNewSubscriber.ReceivedMessages, Contains.Item(message));
        }

        [Test]
        public void GivenRegisteredProviderSendingMessage_RegisteredMultipleProvidersSubscribers_ForDifferentChannels_MultipleSubscribersReceiveCorrectMessages()
        {
            var provider = new Provider();
            _messageBus.Register(Channel, provider);
            var secondProvider = new Provider();
            _messageBus.Register(Channel,secondProvider);
            var thirdProvider = new Provider();
            _messageBus.Register(NewChannel, thirdProvider);

            var subscriber = new Subscriber();
            _messageBus.Register(NewChannel, subscriber);
            var newSubscriber = new Subscriber();
            _messageBus.Register(Channel, newSubscriber);
            var secondNewSubscriber = new Subscriber();
            _messageBus.Register(Channel, secondNewSubscriber);

            var message = new Message();
            var secondMessage = new Message();
            var thirdMessage = new Message();
            _messageBus.MessageFrom(provider, message);
            _messageBus.MessageFrom(secondProvider, secondMessage);
            _messageBus.MessageFrom(thirdProvider, thirdMessage);

            Assert.That(subscriber.ReceivedMessages, Contains.Item(thirdMessage));
            Assert.That(newSubscriber.ReceivedMessages, Contains.Item(message));
            Assert.That(newSubscriber.ReceivedMessages, Contains.Item(secondMessage));
            Assert.That(secondNewSubscriber.ReceivedMessages, Contains.Item(secondMessage));
            Assert.That(secondNewSubscriber.ReceivedMessages, Contains.Item(message));
        }
    }
}
