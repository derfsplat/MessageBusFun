using System.Collections.Generic;
using System.Linq;

namespace MessageBusFun.Core
{
    public class MessageBus
    {
        private readonly IList<Provider> _registeredProviders;
        private readonly IList<Subscriber> _registeredSubscribers;

        public MessageBus()
        {
            _registeredSubscribers = new List<Subscriber>();
            _registeredProviders = new List<Provider>();
        }

        public void Register(string channel, Provider provider)
        {
            _registeredProviders.Add(provider);
            provider.Channel = channel;
        }

        public void Register(string channel, Subscriber subscriber)
        {
            _registeredSubscribers.Add(subscriber);
            subscriber.Channel = channel;
        }

        public void MessageFrom(Provider provider, Message message)
        {
           if (_registeredProviders.Any() && _registeredProviders.Contains(provider))
           {
               foreach (var subscriber in _registeredSubscribers.Where(s => s.Channel == provider.Channel))
               {
                   subscriber.HandleMessage(message);
               }
           }
        }
    }
}
