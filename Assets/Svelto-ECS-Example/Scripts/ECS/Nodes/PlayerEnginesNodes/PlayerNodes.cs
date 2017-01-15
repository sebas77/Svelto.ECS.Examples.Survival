using Svelto.ECS.Example.Components.Base;
using Svelto.ECS.Example.Components.Damageable;
using Svelto.ECS.Example.Components.Gun;
using Svelto.ECS.Example.Components.Player;

namespace Svelto.ECS.Example.Nodes.Player
{
    public class PlayerNode : NodeWithID
    {
        public IHealthComponent        healthComponent;
        public ISpeedComponent         speedComponent;
        public IRigidBodyComponent     rigidBodyComponent;
        public IPositionComponent      positionComponent;
        public IAnimationComponent     animationComponent;
    }

    public class PlayerTargetNode : NodeWithID
    {
        public IDamageEventComponent    damageEventComponent;
        public IHealthComponent         healthComponent;
        public ITargetTypeComponent     targetTypeComponent;
    }
}

namespace Svelto.ECS.Example.Nodes.Gun
{
    public class GunNode : NodeWithID
    {
        public IGunAttributesComponent   gunComponent;
        public IGunFXComponent           gunFXComponent;
        public IGunHitTargetComponent    gunHitTargetComponent;
    }
}
