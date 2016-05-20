using System;
using Svelto.ES;
using Nodes.Sound;
using Components.Damageable;

namespace Engines.Sound
{
    public class DamageSoundEngine : SingleNodeEngine<DamageSoundNode>, IQueryableNodeEngine
    {
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

       void TriggerDeathSound(int sender, int targetID)
       {
            var playerAudioNode =  nodesDB.QueryNode<DamageSoundNode>(sender);
            var playerAudio = playerAudioNode.audioComponent;

            playerAudio.audioSource.PlayOneShot(playerAudio.death);
       }

       void TriggerDamageAudio(int sender, DamageInfo isDamaged)
       {
           var playerAudioNode = nodesDB.QueryNode<DamageSoundNode>(sender);
           var playerAudio = playerAudioNode.audioComponent;

           playerAudio.audioSource.PlayOneShot(playerAudio.damage);
       }

       public IEngineNodeDB nodesDB { set; private get; }
    }
}
