using System;
using System.Collections.Generic;
using System.Linq;

namespace MessageBusFun.Core
{
    internal class ChannelCollection : IChannelCollection
    {
        private readonly IChannelFactory _channelFactory;
        protected ISet<IChannel> Channels { get; private set; }

        private IDictionary<string, IChannel> ChannelNames
        {
            get { return Channels.ToDictionary(chan => chan.Name); }
        }

        public ChannelCollection() : 
            this(new HashSet<IChannel>(), new ChannelFactory())
        {
        }

        public ChannelCollection(ISet<IChannel> channels, IChannelFactory channelFactory)
        {
            Channels = channels;
            _channelFactory = channelFactory;
        }

        public void AddProvider(string name, IProvider provider)
        {
            ThrowWhenProviderExistsWithDifferentChannel(name: name, provider: provider);

            AddProviderToChannel(name, provider);
        }

        private void AddProviderToChannel(string name, IProvider provider)
        {
            var channels = ChannelNames;
            if (channels.ContainsKey(name))
            {
                channels[name].RegisterProvider(provider);
            }
            else
            {
                AddNewChannelWithProvider(name, provider);
            }
        }

        private void AddNewChannelWithProvider(string name, IProvider provider)
        {
            Channels.Add(_channelFactory.CreateWithProvider(name: name, provider: provider));
        }

        protected void ThrowWhenProviderExistsWithDifferentChannel(string name, IProvider provider)
        {
            var existingChannel = ChannelForProvider(provider);
            if (existingChannel != null && existingChannel.Name != name)
            {
                throw new InvalidOperationException("The same provider cannot be associated with multiple channels");
            }
        }

        public IChannel ChannelForProvider(IProvider provider)
        {
            return Channels.SingleOrDefault(x => x.Providers.Contains(provider));
        }

        public IEnumerable<IProvider> ProvidersForChannelWithName(string name)
        {
            return ChannelNames[name].Providers.AsEnumerable();
        }

        public IEnumerable<string> Names()
        {
            return ChannelNames.Keys.AsEnumerable();
        }
    }
}
