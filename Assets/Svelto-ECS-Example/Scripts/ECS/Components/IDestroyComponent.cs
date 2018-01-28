namespace Svelto.ECS.Example.Survive.Components.Enemies
{
    public interface IDestroyComponent
    {
        DispatchOnChange<bool> destroyed { get; }
    }
}