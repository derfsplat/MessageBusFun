using System.Collections.Generic;
using MessageBusFun.Core;
using Moq;
using NUnit.Framework;

namespace MessageBusFun.Tests.Core
{
    [TestFixture]
    public class ChannelsCollectionTests
    {
        private ISet<IChannel> _channelsSet;
        private IChannelCollection _channels;
        private Mock<IChannelFactory> _channelFactoryMock;

        [SetUp]
        public void SetUp()
        {
            _channelsSet = new HashSet<IChannel>();

            _channelFactoryMock = new Mock<IChannelFactory>();
            _channelFactoryMock
                .Setup(x => x.CreateWithProvider(It.IsAny<string>(), It.IsAny<IProvider>()))
                .Returns((string s, IProvider p) => Mock.Of<IChannel>(x => x.Name == s 
                    && x.IsProviderRegistered(p) == true));

            _channels = new ChannelCollection(_channelsSet, _channelFactoryMock.Object);
        }

        [Test]
        public void AddProvider_GivenNonExistentChannelName_WillCreateNewChannelWithProvider()
        {
            const string channelName = "test";
            var provider = Mock.Of<IProvider>();

            _channels.AddProvider(name: channelName, provider: provider);

            _channelFactoryMock.Verify(x => x.CreateWithProvider(channelName, provider), Times.Exactly(1));
            Assert.That(_channelsSet, Has.Count.EqualTo(1));
        }

        [Test]
        public void AddProvider_GivenExistingChannelName_WillAddProviderToExistingChannel()
        {
            const string channelName = "test";
            var provider1 = Mock.Of<IProvider>();
            var provider2 = Mock.Of<IProvider>();

            var channelMock = SetupChannelFactoryChannel(channelName);

            _channels.AddProvider(name: channelName, provider: provider1);
            _channels.AddProvider(name: channelName, provider: provider2);
            
            _channelFactoryMock.Verify(x => x.CreateWithProvider(channelName, provider1), Times.Exactly(1));
            channelMock.Verify(x => x.RegisterProvider(provider2), Times.Exactly(1));
            Assert.That(_channelsSet, Has.Count.EqualTo(1));
        }

        [Test]
        public void AddProvider_GivenNewChannelNameAndProvider_WillAddNewChannel()
        {
            const string channelName1 = "test1";
            const string channelName2 = "test2";
            var provider1 = Mock.Of<IProvider>();
            var provider2 = Mock.Of<IProvider>();

            _channels.AddProvider(name: channelName1, provider: provider1);
            _channels.AddProvider(name: channelName2, provider: provider2);

            Assert.That(_channelsSet, Has.Count.EqualTo(2));
            _channelFactoryMock.Verify(x => x.CreateWithProvider(channelName1, provider1), Times.Exactly(1));
            _channelFactoryMock.Verify(x => x.CreateWithProvider(channelName2, provider2), Times.Exactly(1));
        }

        [Test]
        public void AddProvider_GivenNewChannelNameAndExistingProvider_WillNotAddChannel()
        {
            const string channelName1 = "test1";
            const string channelName2 = "test2";
            var provider = Mock.Of<IProvider>();

            _channels.AddProvider(name: channelName1, provider: provider);
            _channels.AddProvider(name: channelName2, provider: provider);

            _channelFactoryMock.Verify(x => x.CreateWithProvider(channelName2, provider), Times.Never());
        }

        [Test]
        public void Names_GivenChannelWithProvider_ReturnsNameOfChannel()
        {
            const string name = "name";
            _channelsSet.Add(Mock.Of<IChannel>(x => x.Name == name && x.HasProviders == true));

            var names = _channels.Names;

            Assert.That(names, Contains.Item(name));
        }

        [Test]
        public void Names_GivenChannelWithNoProviders_WillNotReturnNameForChannel()
        {
            const string name = "name";
            _channelsSet.Add(Mock.Of<IChannel>(x => x.Name == name && x.HasProviders == false));

            var names = _channels.Names;

            Assert.That(names, Has.None.EqualTo(name));
        }

        private Mock<IChannel> SetupChannelFactoryChannel(string channelName)
        {
            var channelMock = new Mock<IChannel>();
            channelMock.Setup(x => x.Name).Returns(channelName);

            _channelFactoryMock
                .Setup(x => x.CreateWithProvider(channelName, It.IsAny<IProvider>()))
                .Returns(channelMock.Object);

            return channelMock;
        }
    }
}
