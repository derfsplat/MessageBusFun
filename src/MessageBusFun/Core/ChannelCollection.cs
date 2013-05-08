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
            if (ProviderDoesNotExistForDifferentChannel(name: name, provider: provider))
            {
                AddProviderToChannel(name, provider);
            }
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

        protected bool ProviderDoesNotExistForDifferentChannel(string name, IProvider provider)
        {
            var existingChannel = ChannelForProvider(provider);
            return existingChannel == null || existingChannel.Name == name;
        }

        public IChannel ChannelForProvider(IProvider provider)
        {
            return Channels.SingleOrDefault(x => x.IsProviderRegistered(provider));
        }

        public IChannel ChannelWithName(string channelName)
        {
            return Channels.SingleOrDefault(x => x.Name == channelName);
        }

        public IEnumerable<string> Names
        {
            get { return Channels.Where(x => x.HasProviders).Select(x => x.Name); }
        }
    }
}
