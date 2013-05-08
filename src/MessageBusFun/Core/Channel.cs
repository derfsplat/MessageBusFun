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

        public void RegisterProvider(IProvider provider)
        {
            _providers.Add(provider);
        }

        public void DeregisterProvider(IProvider provider)
        {
            if (_providers.Contains(provider))
            {
                _providers.Remove(provider);

                if (!(_providers.Any()))
                {
                    SendChannelUnavailableToSubscribers();
                }
            }
        }

        public void RegisterSubscriber(ISubscriber subscriber)
        {
            _subscribers.Add(subscriber);
        }

        public void DeregisterSubscriber(ISubscriber subscriber)
        {
            _subscribers.Remove(subscriber);
        }

        public void SendMessage(IProvider provider)
        {
            if (IsProviderRegistered(provider))
            {
                SendMessageToSubscribers(provider.Message);
            }
        }

        private void SendMessageToSubscribers(IMessage message)
        {
            foreach (var subscriber in Subscribers)
            {
                subscriber.HandleMessage(message);
            }
        }

        private void SendChannelUnavailableToSubscribers()
        {
            foreach (var subscriber in Subscribers)
            {
                subscriber.HandleChannelUnavailable(Name);
            }
        }

        public bool IsProviderRegistered(IProvider provider)
        {
            return _providers.Contains(provider);
        }

        public bool HasProviders
        {
            get { return _providers.Any(); }
        }
    }
}
