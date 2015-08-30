using System;
using UnityEngine;

namespace Svelto.SignalChain
{
	public class SignalChain
	{
		private Transform _root;
		
		public SignalChain (Transform root)
		{
			_root = root;
		}
		
		public void Send<T>(T notification)
	    {
	        Send(typeof(T), notification);
	    }
		
		public void Send<T>()
	    {
	        Send(typeof(T));
	    }
		
		void Send(Type notification)
	    {
	        Send(notification, null);
	    }
		
		public void Send(Type notificationType, object notification)
	    {
	    	MonoBehaviour[] behaviours = _root.GetComponents<MonoBehaviour>();
	    
	        foreach (MonoBehaviour behaviour in behaviours)
				if (behaviour is IChainListener)
					(behaviour as IChainListener).Listen(notification != null ? notification : notificationType);
	    }
		
		public void Broadcast<T>()
	    {
	        Broadcast(typeof(T));
	    }
		
		public void Broadcast<T>(T notification)
	    {
	        Broadcast(typeof(T), notification, false);
	    }
		
		void Broadcast(Type notification)
	    {
	        Broadcast(notification, null, false);
	    }
		
		public void Broadcast<T>(T notification, bool notifyDisabled)
	    {
	        Broadcast(typeof(T), notification, notifyDisabled);
	    }
		
		public void Broadcast(Type notification, bool notifyDisabled)
	    {
	        Broadcast(notification, null, notifyDisabled);
	    }
		
		private void Broadcast(Type notificationType, object notification, bool notifyDisabled)
	    {
	        foreach (MonoBehaviour behaviour in _root.GetComponentsInChildren<MonoBehaviour>(notifyDisabled))
				if (behaviour is IChainListener)
					(behaviour as IChainListener).Listen(notification != null ? notification : notificationType);
	    }
		
		public void DeepBroadcast<T>()
	    {
	        DeepBroadcast(typeof(T));
	    }
		
		public void DeepBroadcast<T>(T notification)
	    {
	        DeepBroadcast(typeof(T), notification);
	    }
		
		void DeepBroadcast(Type notification)
	    {
	        DeepBroadcast(notification, null);
	    }
		
		private void DeepBroadcast(Type notificationType, object notification)
	    {
			Broadcast(notificationType, notification, true);
		}
	}
}

