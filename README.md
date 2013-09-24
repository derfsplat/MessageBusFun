Message Bus Fun
===============

Your challenge – should you choose to accept it – is to design a component according to the
following criteria.

Non-functional acceptance criteria.
-----------------------------------

###Overview
Assume – in a corporate context - that you are creating a small solution to demonstrate your
design philosophy and essential concepts to a trainee. The solution should therefore contain an
example illustrating each fundamental concept or mechanism that you consider to be important.

###Submission format.
Submissions should take the form of a pull request or a zipped solution; any Visual Studio
version from 2010 on is permissible. As far as possible, the
solution should be useable with no special preparation of the development environment. Any
instructions or usage notes should be in added to the beginning of this readme.

###Language, framework, and dependencies.
The solution should be authored in C# and target the .Net Framework version 3.5 or above.
References should be limited to either those available by default (given Visual Studio Ultimate
Edition), or to commonly known additions to the development environment that can be included
via NuGet.

###Target Operating Environment
Assume that the component is to be delivered in binary form to multiple other development
teams for incorporation into various applications. The component is intended to be utilised
directly in-process; it is not necessary to manage the exposure of any functionality or state via
any mechanism other than by the use of visibility modifiers. The applications that consume
the component will be running in either a server or client context, so no dependency on any
particular host platform – such as IIS – should exist.

Functional description.
-----------------------
* The component is intended to manage the routing of messages from multiple providers -
grouped into channels - to multiple subscribers. The reliability of the routing is key. Any API or
interface necessary to the routing mechanism are the domain of the component.
* Consuming applications should be able to register and deregister both subscribers and
providers.
* Providers will nominate a channel name (a string) upon registration. Multiple providers
may nominate the same channel name. A provider cannot change channel names without
deregistering.
* The consuming application should be able to read a list of available channels. An available
channel is one for which at least one provider is currently registered.
* Consuming applications should be able to register a subscriber to receive messages from any
available channel/s without limitation.
* Once registered to an available channel, the subscriber will receive all messages sent by
registered providers on that channel.
* The subscriber needs to identify which channel a message originates from, but not the identity
of the provider.
* Any subscribers are notified if a channel becomes unavailable, but are not automatically
unsubscribed. If the channel becomes available again, subscribers still registered will once again
receive messages from it.
* Multiple attempts to register a subscriber to receive messages to the same channel should have
no effect.
* Multiple attempts to deregister a subscriber from receiving messages from the same channel
should have no effect.
* Attempts to deregister a subscriber which is not registered should have no effect.
* A consuming application should be able to deregister a subscriber from receiving messages from
any channels without limitation.
* A consuming application should be able to deregister a provider from publishing messages.
* Multiple attempts to deregister the same provider from publishing messages should have no
effect.
* Attempts to deregister a provider which is not registered should have no effect.
