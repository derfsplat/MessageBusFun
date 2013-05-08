using System.Collections.Generic;

namespace MessageBusFun
{
    public interface IMessageBus
    {
        IEnumerable<string> Channels { get; }
        void RegisterProvider(IProvider provider, string channelName);
        void Publish(IProvider provider);
        void DeregisterProvider(IProvider provider);
        void RegisterSubscriber(ISubscriber subscriber, string channelName);
        void DeregisterSubscriber(ISubscriber subscriber, string channelName);
    }
}
