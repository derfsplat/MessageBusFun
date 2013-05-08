using System.Collections.Generic;

namespace MessageBusFun.Core
{
    public interface IMessageBus
    {
        IEnumerable<string> Channels { get; }

        void RegisterProvider(IProvider provider, string channelName);
    }
}
