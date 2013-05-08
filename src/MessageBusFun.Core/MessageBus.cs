using System.Collections.Generic;

namespace MessageBusFun.Core
{
    internal class MessageBus : IMessageBus
    {
        private readonly IChannelProviderCollection _channels;

        public MessageBus(IChannelProviderCollection channels)
        {
            _channels = channels;
        }

        public MessageBus() : this(new ChannelProviderCollection()) {}

        public IEnumerable<string> Channels { get { return _channels.Names(); } }

        public void RegisterProvider(IProvider provider, string channelName)
        {
            _channels.Add(channelName, provider);
        }
    }
}
