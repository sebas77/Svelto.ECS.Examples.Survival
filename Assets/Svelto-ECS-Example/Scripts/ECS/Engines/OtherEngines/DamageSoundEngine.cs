using Svelto.ECS.Example.Survive.Nodes.Sound;
using Svelto.ECS.Example.Survive.Components.Damageable;
using System;

namespace Svelto.ECS.Example.Survive.Engines.Sound.Damage
{
    public class DamageSoundEngine : IEngine, IQueryableNodeEngine, IStep<PlayerDamageInfo>
    {
        public IEngineNodeDB nodesDB { set; private get; }

        void TriggerDeathSound(int targetID)
        {
            var audioNode =  nodesDB.QueryNode<DamageSoundNode>(targetID);
            var audioComponent = audioNode.audioComponent;

            audioComponent.audioSource.PlayOneShot(audioComponent.death);
        }

        void TriggerDamageAudio(int sender)
        {
           var audioNode = nodesDB.QueryNode<DamageSoundNode>(sender);
           var audioComponent = audioNode.audioComponent;

           audioComponent.audioSource.PlayOneShot(audioComponent.damage);
        }

        public void Step(ref PlayerDamageInfo token, Enum condition)
        {
            if ((DamageCondition)condition == DamageCondition.damage)
                TriggerDamageAudio(token.entityDamaged);
            else
                TriggerDeathSound(token.entityDamaged);
        }
    }
}
