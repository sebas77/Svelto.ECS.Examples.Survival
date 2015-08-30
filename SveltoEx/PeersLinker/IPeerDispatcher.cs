namespace Svelto.PeersLinker
{
    public interface IPeerDispatcher: IPeer
	{
		event System.Action<object> notify;
	}
}
