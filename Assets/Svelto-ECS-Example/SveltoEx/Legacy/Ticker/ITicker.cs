namespace Svelto.Ticker.Legacy
{
    public interface ITicker
    {
        void Add(ITickableBase tickable);
        void Remove(ITickableBase tickable);
    }
}
