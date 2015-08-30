using System;

namespace Svelto.SignalChain
{
	public interface IChainListener
	{
		void Listen<T>(T message);
	}
}

