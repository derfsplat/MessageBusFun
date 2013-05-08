using System.Collections.Generic;
using System.Linq;

namespace MessageBusFun.Core
{
    public class Channel : IChannel
    {
        private readonly ISet<IProvider> _providers;
        private readonly ISet<ISubscriber> _subscribers;

        public IEnumerable<IProvider> Providers { get { return _providers.AsEnumerable(); } }
        public IEnumerable<ISubscriber> Subscribers { get { return _subscribers.AsEnumerable(); } }

        public Channel(string name, IProvider provider) : this(name, provider, new HashSet<IProvider>(), new HashSet<ISubscriber>()) {}

        public Channel(string name, IProvider provider, ISet<IProvider> providers, ISet<ISubscriber> subscribers)
        {
            _subscribers = subscribers;
            _providers = providers;

            Name = name;
            _providers.Add(provider);
        }

        public string Name { get; private set; }

        public void SendMessage(IProvider provider)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterProvider(IProvider provider)
        {
            throw new System.NotImplementedException();
        }

        public void RegisterSubscriber(ISubscriber provider)
        {
            throw new System.NotImplementedException();
        }
    }
}
