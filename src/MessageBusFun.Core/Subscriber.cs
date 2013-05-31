using System.Collections.Generic;

namespace MessageBusFun.Core
{
    public class Subscriber
    {
        private readonly IList<Message> _receivedMessages;

        public string Channel { get; set; }

        public Subscriber()
        {
            _receivedMessages = new List<Message>();
        }

        public IEnumerable<Message> ReceivedMessages
        {
            get { return _receivedMessages; }
        }

        public void HandleMessage(Message message)
        {
            _receivedMessages.Add(message);
        }
    }
}
