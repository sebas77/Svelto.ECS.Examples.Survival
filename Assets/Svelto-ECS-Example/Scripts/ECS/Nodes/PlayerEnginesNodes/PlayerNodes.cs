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
        public IRemoveEntityComponent  removeEntityComponent;
    }

    public class PlayerTargetNode : NodeWithID
    {
        public IDamageEventComponent    damageEventComponent;
        public IHealthComponent         healthComponent;
        public ITargetTypeComponent     targetTypeComponent;
    }
}

namespace Nodes.Gun
{
    public class GunNode : NodeWithID
    {
        public IGunAttributesComponent   gunComponent;
        public IGunFXComponent           gunFXComponent;
        public IGunHitTargetComponent    gunHitTargetComponent;
    }
}
