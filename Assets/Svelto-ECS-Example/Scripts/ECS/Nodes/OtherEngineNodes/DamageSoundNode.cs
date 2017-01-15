using Svelto.ECS.Example.Components.Base;
using Svelto.ECS.Example.Components.Damageable;

namespace Svelto.ECS.Example.Nodes.Sound
{
    public class DamageSoundNode: NodeWithID
    {
        public IDamageSoundComponent    audioComponent;
        public IHealthComponent         healthComponent;
    }
}
