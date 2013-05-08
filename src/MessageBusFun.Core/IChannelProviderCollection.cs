using System.Collections.Generic;

namespace MessageBusFun.Core
{
    internal interface IChannelProviderCollection
    {
        void Add(string name, IProvider provider);
        IEnumerable<string> Names();
        IEnumerable<IProvider> ProvidersForChannel(string name);
    }
}