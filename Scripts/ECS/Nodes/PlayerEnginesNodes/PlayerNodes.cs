using GunComponents;
using PlayerComponents;
using SharedComponents;
using Svelto.ES;
using UnityEngine;

namespace PlayerEngines
{
    public class PlayerNode : INode
    {
        public IHealthComponent        healthComponent;
        public ISpeedComponent         speedComponent;
        public IRigidBodyComponent     rigidBodyComponent;
        public IPositionComponent      positionComponent;
        public IAnimationComponent     animationComponent;
    }

    public class PlayerGunNode : INode
    {
        public IGunComponent gunComponent;
        public IGunFXComponent gunFXComponent;
    }

    public class PlayerTargetNode : INodeWithReferenceID<GameObject>
    {
        public IDamageEventComponent    damageEventComponent;
        public IHealthComponent         healthComponent;
        public ITargetTypeComponent     targetTypeComponent;

        public PlayerTargetNode(GameObject ID) { this.ID = ID; }
        public GameObject ID { get; private set; }
    }
}
