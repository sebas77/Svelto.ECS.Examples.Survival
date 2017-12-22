using Svelto.ECS.Example.Survive.Components.Base;
using Svelto.ECS.Example.Survive.Components.Gun;
using Svelto.ECS.Example.Survive.Components.Player;

namespace Svelto.ECS.Example.Survive.EntityViews.Player
{
    public class PlayerEntityView : EntityView<PlayerEntityView>
    {
        public ISpeedComponent         speedComponent;
        public IRigidBodyComponent     rigidBodyComponent;
        public IPositionComponent      positionComponent;
        public IAnimationComponent     animationComponent;
    }

    public class PlayerTargetEntityView : EntityView<PlayerTargetEntityView>
    {
        public ITargetTypeComponent     targetTypeComponent;
    }
}

namespace Svelto.ECS.Example.Survive.EntityViews.Gun
{
    public class GunEntityView : EntityView<GunEntityView>
    {
        public IGunAttributesComponent   gunComponent;
        public IGunFXComponent           gunFXComponent;
        public IGunHitTargetComponent    gunHitTargetComponent;
    }
}
