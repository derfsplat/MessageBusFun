using System.Collections.Generic;

namespace MessageBusFun.Core
{
    internal interface IChannelCollection
    {
        void AddProvider(string name, IProvider provider);
        IEnumerable<string> Names { get; }
        IChannel ChannelForProvider(IProvider provider);
        IChannel ChannelWithName(string channelName);
    }
}
