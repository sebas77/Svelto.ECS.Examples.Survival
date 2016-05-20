using Components.Base;
using Components.Damageable;
using Components.Gun;
using Components.Player;
using Svelto.ES;

namespace Nodes.Player
{
    public class PlayerNode : NodeWithID
    {
        public IHealthComponent        healthComponent;
        public ISpeedComponent         speedComponent;
        public IRigidBodyComponent     rigidBodyComponent;
        public IPositionComponent      positionComponent;
        public IAnimationComponent     animationComponent;
    }

    public class PlayerGunNode : NodeWithID
    {
        public IGunComponent gunComponent;
        public IGunFXComponent gunFXComponent;
    }

    public class PlayerTargetNode : NodeWithID
    {
        public IDamageEventComponent    damageEventComponent;
        public IHealthComponent         healthComponent;
        public ITargetTypeComponent     targetTypeComponent;
    }
}
