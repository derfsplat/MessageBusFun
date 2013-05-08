namespace MessageBusFun.Core
{
    public class ChannelFactory : IChannelFactory
    {
        public IChannel CreateWithProvider(string name, IProvider provider)
        {
            return new Channel(name: name, provider: provider);
        }
    }
}
