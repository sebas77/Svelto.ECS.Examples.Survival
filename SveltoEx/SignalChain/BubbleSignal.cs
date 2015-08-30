using System;
using UnityEngine;

namespace Svelto.SignalChain
{
	public class BubbleSignal<T>
	{
		SignalChain _signalChain;
				
		public BubbleSignal (Transform node)
		{
			Transform 	root = node;
			
			while (root != null && RootIsFound(root) == false)	
				root = root.parent;
				
			_signalChain = new SignalChain(root == null ? node : root);
		}
		
		//  The notification of type M is broadcasted 
		//	to all the active children of type IChainListener
		
		public void Dispatch<M>()
		{
			_signalChain.Broadcast<M>();
		}
		
		//  The notification of type M is broadcasted 
		//	to all the active children of type IChainListener
		
		public void Dispatch<M>(M notification)
		{
			_signalChain.Broadcast<M>(notification);
		}
		
		//  The notification of type M is broadcasted 
		//	to all the children of type IChainListener
		
		public void DeepDispatch<M>()
		{
			_signalChain.DeepBroadcast<M>();
		}
		
		//  The notification of type M is broadcasted 
		//	to all the children of type IChainListener
		
		public void DeepDispatch<M>(M notification)
		{
			_signalChain.DeepBroadcast<M>(notification);
		}
		
		//  The notification of type T is sent 
		//	to the root components of type IChainListener
		
		public void TargetedDispatch<M>()
		{
			_signalChain.Send<M>();
		}
		
		//  The notification of type T is sent 
		//	to the root components of type IChainListener
		
		public void TargetedDispatch<M>(M notification)
		{
			_signalChain.Send<M>(notification);
		}
		
		private bool RootIsFound(Transform node)
		{
			return Array.FindIndex<Component>(node.GetComponents<Component>(), obj => { 
				return obj is T; 
			}) != -1;
		}
	}
}

