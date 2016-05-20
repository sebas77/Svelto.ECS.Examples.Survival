using Components.Base;
using Components.Damageable;
using Svelto.ES;

namespace Nodes.Sound
{
    public class DamageSoundNode: NodeWithID
    {
        public IDamageSoundComponent    audioComponent;
        public IHealthComponent         healthComponent;
    }
}
