using MessageBusFun.Core;
using NUnit.Framework;

namespace MessageBusFun.Tests.Core
{
    [TestFixture]
    public class MessageBusFactoryTests
    {
        [Test]
        public void Create_ReturnsIMessageBus()
        {
            var factory = new MessageBusFactory();

            var messageBus = factory.Create();

            Assert.That(messageBus, Is.InstanceOf<IMessageBus>());
        }
    }
}
