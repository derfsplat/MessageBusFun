using System.Collections.Generic;

namespace MessageBusFun.Core
{
    internal interface IChannelCollection
    {
        void AddProvider(string name, IProvider provider);
        IEnumerable<string> Names();
        IEnumerable<IProvider> ProvidersForChannelWithName(string name);
        IChannel ChannelForProvider(IProvider provider);
    }
}
