using System;
using Svelto.ES;
using Nodes.Sound;
using Components.Damageable;

namespace Engines.Sound.Damage
{
    public class DamageSoundEngine : SingleNodeEngine<DamageSoundNode>, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }

        override protected void Add(DamageSoundNode node)
        {
            var healthComponent = (node as DamageSoundNode).healthComponent;

            healthComponent.isDead.subscribers += TriggerDeathSound;
            healthComponent.isDamaged.subscribers += TriggerDamageAudio;
        }

        override protected void Remove(DamageSoundNode node)
        {
            var healthComponent = (node as DamageSoundNode).healthComponent;

            healthComponent.isDead.subscribers -= TriggerDeathSound;
            healthComponent.isDamaged.subscribers -= TriggerDamageAudio;
        }

       void TriggerDeathSound(int targetID)
       {
            var audioNode =  nodesDB.QueryNode<DamageSoundNode>(targetID);
            var audioComponent = audioNode.audioComponent;

            audioComponent.audioSource.PlayOneShot(audioComponent.death);
       }

       void TriggerDamageAudio(int sender, DamageInfo isDamaged)
       {
           var audioNode = nodesDB.QueryNode<DamageSoundNode>(sender);
           var audioComponent = audioNode.audioComponent;

           audioComponent.audioSource.PlayOneShot(audioComponent.damage);
       }
    }
}
