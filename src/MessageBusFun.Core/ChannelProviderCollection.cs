using System;
using System.Collections.Generic;
using System.Linq;

namespace MessageBusFun.Core
{
    internal class ChannelProviderCollection : IChannelProviderCollection
    {
        protected IDictionary<string, ISet<IProvider>> Channels { get; private set; }

        public ChannelProviderCollection() : 
            this(new Dictionary<string, ISet<IProvider>>())
        {
        }

        public ChannelProviderCollection(IDictionary<string, ISet<IProvider>> dictionary)
        {
            Channels = dictionary;
        }

        public void Add(string name, IProvider provider)
        {
            ThrowWhenProviderExistsWithDifferentChannel(name: name, provider: provider);

            if (Channels.ContainsKey(name))
            {
                Channels[name].Add(provider);
            }
            else
            {
                Channels.Add(name, CreateSetWith(provider));
            }
        }

        protected void ThrowWhenProviderExistsWithDifferentChannel(string name, IProvider provider)
        {
            var existingChannel = ChannelFor(provider);
            if (existingChannel != null && existingChannel != name)
            {
                throw new InvalidOperationException("The same provider cannot be associated with multiple channels");
            }
        }

        protected HashSet<IProvider> CreateSetWith(IProvider provider)
        {
            return new HashSet<IProvider> {provider};
        }

        protected string ChannelFor(IProvider provider)
        {
            return Channels.SingleOrDefault(x => x.Value.Contains(provider)).Key;
        }

        public IEnumerable<IProvider> ProvidersForChannel(string name)
        {
            return Channels[name].AsEnumerable();
        }

        public IEnumerable<string> Names()
        {
            return Channels.Keys.AsEnumerable();
        }
    }
}
