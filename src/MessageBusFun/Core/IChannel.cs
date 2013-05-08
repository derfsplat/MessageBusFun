using System.Collections.Generic;

namespace MessageBusFun.Core
{
    public interface IChannel
    {
        string Name { get; }
        IEnumerable<IProvider> Providers { get; }
        IEnumerable<ISubscriber> Subscribers { get; } 
        void SendMessage(IProvider provider);
        void RegisterProvider(IProvider provider);
        void RegisterSubscriber(ISubscriber provider);
    }
}
