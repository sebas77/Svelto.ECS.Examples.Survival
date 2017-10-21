using Svelto.ECS.Example.Survive.Components.Base;
using Svelto.ECS.Example.Survive.Components.Damageable;

namespace Svelto.ECS.Example.Survive.Nodes.Sound
{
    public class DamageSoundNode: NodeWithID
    {
        public IDamageSoundComponent    audioComponent;
        public IHealthComponent         healthComponent;
    }
}
