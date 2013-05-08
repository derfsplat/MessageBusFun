namespace MessageBusFun.Core
{
    public class MessageBusFactory : IMessageBusFactory
    {
        public IMessageBus Create()
        {
            return new MessageBus();
        }
    }
}
