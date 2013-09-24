using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBusFun.Core
{
	/// <summary>
	/// Provides thread-safe pub sub
	/// </summary>
	/// <remarks>Magic Bus</remarks>
	public class MessageBus
	{
		//immutable types should be readonly- this is for your own protection!
		//concurrent dictionary gives us thread-safe acces for free (can you spot where we're not thread safe?)
		//hint http://stackoverflow.com/a/6601832/251983
		readonly ConcurrentDictionary<Tuple<Type, string>, List<object>> subscribers = new ConcurrentDictionary<Tuple<Type, string>, List<object>>();

		public void Register<T>(string context = "")
		{
			var key = new Tuple<Type, string>(typeof(T), context);
			subscribers.AddOrUpdate(key, new List<object>(), (k, v) => v);
		}

		public void Unregister<T>(string context = "")
		{
			List<object> subs;
			if(!subscribers.TryRemove(new Tuple<Type, string>(typeof(T), context), out subs))
			{
				//log to favorite log provider
				Console.WriteLine("Failed to remove channel type {0} context: {1}", typeof(T), context);
			}
		}

		//return IEnumerable when you do not need random access to a collection
		//prefer interfaces over concrete types for public interfaces
		//prefer IEnumerable over (I)List as the interface is less demanding with respect to the external contract you're making with your client
		//(i.e. you have less to do to meet the contract and more flexibility about return function. For example, let's say accessing a channel 
		//becomes an expensive operation (e.g. pulling from a db), you could yield return your enumeration)
		public IEnumerable<Tuple<Type, string>> Channels
		{
			get
			{
				return subscribers.Keys;
			}
		}

		public Action Subscribe<T>(Action<T> listener, string context = "")
		{
			var key = listener.ToKey(context); //new Tuple<Type, string>(typeof(T), context);
			subscribers.AddOrUpdate(key, new List<object>(), (k, v) => { v.Add(listener); return v; });
			return () =>
			{
				List<object> listeners = new List<object>();
				subscribers.TryGetValue(new Tuple<Type, string>(typeof(T), context), out listeners);
				//prefer conditionals that 'yield' a return, avoid nesting http://www.beenishkhan.net/2012/05/27/if-statements-are-bad-for-code-avoid-them/
				if(listeners == null) return;
				//not thread safe
				listeners.Remove(listener);
			};
		}

		public void Publish<T>(T message, string context = "")
		{
			List<object> listeners = new List<object>();
			subscribers.TryGetValue(message.ToKey(context), out listeners);
			if(listeners == null) return;
			foreach(var listener in listeners.Cast<Action<T>>())
				listener(message);
		}
	}

	public static class KeyExtensions
	{
		//extension methods are great for DRY'ing up code
		//TODO: consider base "Message" type so that we're not extending every object
		public static Tuple<Type, string> ToKey(this object obj, string context)
		{
			Type t = obj.GetType();
			//if we had a base Message type, we could extend Action<T> specifically, making this code less hacky
			if(t.GenericTypeArguments.Any()) t = t.GenericTypeArguments.First(); //e.g. Action<T>
			return new Tuple<Type, string>(t, context);
		}
	}
}
