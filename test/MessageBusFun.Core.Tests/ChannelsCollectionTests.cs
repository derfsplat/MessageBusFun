using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace MessageBusFun.Core.Tests
{
    [TestFixture]
    public class ChannelsCollectionTests
    {
        private Dictionary<string, ISet<IProvider>> _channelsDictionary;
        private ChannelProviderCollection _channels;

        [SetUp]
        public void SetUp()
        {
            _channelsDictionary = new Dictionary<string, ISet<IProvider>>();

            _channels = new ChannelProviderCollection(_channelsDictionary);
        }

        [Test]
        public void Add_GivenNonExistentChannelName_WillCreateNewCollectionForChannelWithProvider()
        {
            const string channelName = "test";
            var provider = Mock.Of<IProvider>();

            _channels.Add(name: channelName, provider: provider);
            
            Assert.That(_channelsDictionary, Has.Count.EqualTo(1));
            Assert.That(_channelsDictionary.ContainsKey(channelName));
            Assert.That(_channelsDictionary[channelName], Has.Count.EqualTo(1));
            Assert.That(_channelsDictionary[channelName], Contains.Item(provider));
        }

        [Test]
        public void Add_GivenExistingChannelName_WillAddProviderToExistingCollectionForChannel()
        {
            const string channelName = "test";
            var provider1 = Mock.Of<IProvider>();
            var provider2 = Mock.Of<IProvider>();

            _channels.Add(name: channelName, provider: provider1);
            _channels.Add(name: channelName, provider: provider2);

            Assert.That(_channelsDictionary, Has.Count.EqualTo(1));
            Assert.That(_channelsDictionary[channelName], Has.Count.EqualTo(2));
            Assert.That(_channelsDictionary[channelName], Contains.Item(provider1));
            Assert.That(_channelsDictionary[channelName], Contains.Item(provider2));
        }

        [Test]
        public void Add_GivenExistingChannelNameAndProviderForChannel_WillOnlyRetainSingleInstanceOfProvider()
        {
            const string channelName = "test";
            var provider = Mock.Of<IProvider>();

            _channels.Add(name: channelName, provider: provider);
            _channels.Add(name: channelName, provider: provider);

            Assert.That(_channelsDictionary[channelName], Has.Count.EqualTo(1));
            Assert.That(_channelsDictionary[channelName], Contains.Item(provider));
        }

        [Test]
        public void Add_GivenNewChannelNameAndProvider_WillAddProviderToNewCollectionForChannel()
        {
            const string channelName1 = "test1";
            const string channelName2 = "test2";
            var provider1 = Mock.Of<IProvider>();
            var provider2 = Mock.Of<IProvider>();

            _channels.Add(name: channelName1, provider: provider1);
            _channels.Add(name: channelName2, provider: provider2);

            Assert.That(_channelsDictionary, Has.Count.EqualTo(2));
            Assert.That(_channelsDictionary[channelName1], Has.Count.EqualTo(1));
            Assert.That(_channelsDictionary[channelName1], Contains.Item(provider1));
            Assert.That(_channelsDictionary[channelName2], Has.Count.EqualTo(1));
            Assert.That(_channelsDictionary[channelName2], Contains.Item(provider2));
        }

        [Test]
        public void Add_GivenNewChannelNameAndExistingProvider_WillThrowException()
        {
            const string channelName1 = "test1";
            const string channelName2 = "test2";
            var provider = Mock.Of<IProvider>();

            _channels.Add(name: channelName1, provider: provider);
            TestDelegate addSecondChannel = () => 
                _channels.Add(name: channelName2, provider: provider);

            Assert.That(addSecondChannel, Throws.Exception);
        }
    }
}
