using MessageBusFun.Core;

namespace MessageBusFun
{
    public class MessageBusFactory : IMessageBusFactory
    {
        public IMessageBus Create()
        {
            return new MessageBus();
        }
    }
}
