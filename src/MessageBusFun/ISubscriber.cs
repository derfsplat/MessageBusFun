namespace MessageBusFun
{
    public interface ISubscriber
    {
        void HandleMessage(IMessage message);
        void HandleChannelUnavailable(string channelName);
    }
}