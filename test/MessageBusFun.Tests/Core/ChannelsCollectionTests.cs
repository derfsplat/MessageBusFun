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

        //[Test]
        //public void AddProvider_GivenExistingChannelName_WillAddProviderToExistingChannel()
        //{
        //    const string channelName = "test";
        //    var provider1 = Mock.Of<IProvider>();
        //    var provider2 = Mock.Of<IProvider>();

        //    _channels.AddProvider(name: channelName, provider: provider1);
        //    _channels.AddProvider(name: channelName, provider: provider2);

        //    Assert.That(_channelsSet.Single().Providers, Has.Count.EqualTo(2));
        //    Assert.That(_channelsSet.Single().Providers, Contains.Item(provider1));
        //    Assert.That(_channelsSet.Single().Providers, Contains.Item(provider2));
        //}

        //[Test]
        //public void AddProvider_GivenNewChannelNameAndProvider_WillAddProviderToNewCollectionForChannel()
        //{
        //    const string channelName1 = "test1";
        //    const string channelName2 = "test2";
        //    var provider1 = Mock.Of<IProvider>();
        //    var provider2 = Mock.Of<IProvider>();

        //    _channels.AddProvider(name: channelName1, provider: provider1);
        //    _channels.AddProvider(name: channelName2, provider: provider2);

        //    Assert.That(_channelsDictionary, Has.Count.EqualTo(2));
        //    Assert.That(_channelsDictionary[channelName1], Has.Count.EqualTo(1));
        //    Assert.That(_channelsDictionary[channelName1], Contains.Item(provider1));
        //    Assert.That(_channelsDictionary[channelName2], Has.Count.EqualTo(1));
        //    Assert.That(_channelsDictionary[channelName2], Contains.Item(provider2));
        //}

        //[Test]
        //public void Add_GivenNewChannelNameAndExistingProvider_WillThrowException()
        //{
        //    const string channelName1 = "test1";
        //    const string channelName2 = "test2";
        //    var provider = Mock.Of<IProvider>();

        //    _channels.AddProvider(name: channelName1, provider: provider);
        //    TestDelegate addSecondChannel = () => 
        //        _channels.AddProvider(name: channelName2, provider: provider);

        //    Assert.That(addSecondChannel, Throws.Exception);
        //}
    }
}
