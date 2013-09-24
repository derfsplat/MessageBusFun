using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace MessageBusFun.Core.Tests
{
	[TestFixture]
	public class MessageBusTests
	{
		class Message
		{
			public string Content { get; set; }
		}

		[Test]
		//SystemUnderTest_MethodUnderTest_OptionalContext_Expectation
		public void MessageBus_Register_Registers()
		{
			//arrange
			var sut = new MessageBus();
			
			//act
			sut.Register<Message>();

			//assert
			Assert.AreEqual(1, sut.Channels.Count());
			Assert.AreEqual(typeof(Message), sut.Channels.First().Item1);
		}

		[Test]
		public void MessageBus_Publish_PublishesToAllSubscribers()
		{
			var sut = new MessageBus();
			sut.Register<Message>();
			bool called = false;
			var unsubscribe = sut.Subscribe<Message>(m =>
				{
					Assert.AreEqual("Eureka", m.Content);
					called = true;
				});

			sut.Publish<Message>(new Message() { Content = "Eureka" });
			unsubscribe();

			Assert.AreEqual(true, called);
		}

		[Test]
		public void MessageBus_Subscribe_WhenUnsubscribeActionCalled_ListenerIsUnsubscribed()
		{
			var sut = new MessageBus();
			sut.Register<Message>();
			int numCalls = 0;		
			var unsubscribe = sut.Subscribe<Message>(m =>
			{
				numCalls++;
			});

			sut.Publish<Message>(new Message() { Content = "Eureka" });
			unsubscribe();
			sut.Publish<Message>(new Message() { Content = "Eureka2" });

			Assert.AreEqual(1, numCalls);
		}

		[Test]
		public void MessageBus_Unregister_RemovesChannel()
		{
			var sut = new MessageBus();
			sut.Register<Message>();

			sut.Unregister<Message>();

			Assert.AreEqual(0, sut.Channels.Count());
		}
	}
}
