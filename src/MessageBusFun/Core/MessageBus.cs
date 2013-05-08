using System.Collections.Generic;

namespace MessageBusFun.Core
{
    internal class MessageBus : IMessageBus
    {
        private readonly IChannelCollection _channels;

        public MessageBus(IChannelCollection channels)
        {
            _channels = channels;
        }

        public MessageBus() : this(new ChannelCollection()) {}

        public IEnumerable<string> Channels { get { return _channels.Names; } }

        public void RegisterProvider(IProvider provider, string channelName)
        {
            _channels.AddProvider(channelName, provider);
        }

        public void Publish(IProvider provider)
        {
            _channels.ChannelForProvider(provider);
        }

        public void DeregisterProvider(IProvider provider) { }

        public void RegisterSubscriber(ISubscriber subscriber, string channelName) {}

        public void DeregisterSubscriber(ISubscriber subscriber, string channelName) {}
    }
}
