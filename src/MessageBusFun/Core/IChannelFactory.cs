namespace MessageBusFun.Core
{
    public interface IChannelFactory
    {
        IChannel CreateWithProvider(string name, IProvider provider);
    }
}