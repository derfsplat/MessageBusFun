using System.Collections.Generic;

namespace MessageBusFun.Core
{
    public interface IChannel
    {
        string Name { get; }
        
        IEnumerable<IProvider> Providers { get; }
        IEnumerable<ISubscriber> Subscribers { get; }
        
        void RegisterProvider(IProvider provider);
        void DeregisterProvider(IProvider provider);
        void RegisterSubscriber(ISubscriber subscriber);
        void DeregisterSubscriber(ISubscriber subscriber);
        
        void SendMessage(IProvider provider);

        bool IsProviderRegistered(IProvider provider);
        bool HasProviders { get; }
    }
}
