namespace MessageBusFun.Core
{
    public interface IMessageBusFactory
    {
        IMessageBus Create();
    }
}
