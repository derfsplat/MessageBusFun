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

        public void DeregisterProvider(IProvider provider)
        {
            var channel = _channels.ChannelForProvider(provider);
            if (channel != null)
            {
                channel.DeregisterProvider(provider);
            }
        }

        public void RegisterSubscriber(ISubscriber subscriber, string channelName)
        {
            var channel = _channels.ChannelWithName(channelName);
            if (channel != null)
            {
                channel.RegisterSubscriber(subscriber);
            }
        }

        public void DeregisterSubscriber(ISubscriber subscriber, string channelName)
        {
            var channel = _channels.ChannelWithName(channelName);
            if (channel != null)
            {
                channel.DeregisterSubscriber(subscriber);
            }
        }

        public void Publish(IProvider provider)
        {
            var channel = _channels.ChannelForProvider(provider);
            if (channel != null)
            {
                channel.SendMessage(provider);
            }
        }
    }
}
