using Svelto.ECS.Example.Survive.Components.Base;
using Svelto.ECS.Example.Survive.Components.Damageable;
using Svelto.ECS.Example.Survive.Components.Gun;
using Svelto.ECS.Example.Survive.Components.Player;

namespace Svelto.ECS.Example.Survive.Nodes.Player
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
        public IHealthComponent         healthComponent;
        public ITargetTypeComponent     targetTypeComponent;
    }
}

namespace Svelto.ECS.Example.Survive.Nodes.Gun
{
    public class GunNode : NodeWithID
    {
        public IGunAttributesComponent   gunComponent;
        public IGunFXComponent           gunFXComponent;
        public IGunHitTargetComponent    gunHitTargetComponent;
    }
}
