using Svelto.ECS.Example.Nodes.Sound;
using Svelto.ECS.Example.Components.Damageable;

namespace Svelto.ECS.Example.Engines.Sound.Damage
{
    public class DamageSoundEngine : SingleNodeEngine<DamageSoundNode>, IQueryableNodeEngine
    {
        public IEngineNodeDB nodesDB { set; private get; }

        override protected void Add(DamageSoundNode node)
        {
            var healthComponent = (node as DamageSoundNode).healthComponent;

            healthComponent.isDead.NotifyOnDataChange(TriggerDeathSound);
            healthComponent.isDamaged.subscribers += TriggerDamageAudio;
        }

        override protected void Remove(DamageSoundNode node)
        {
            var healthComponent = (node as DamageSoundNode).healthComponent;

            healthComponent.isDead.StopNotifyOnDataChange(TriggerDeathSound);
            healthComponent.isDamaged.subscribers -= TriggerDamageAudio;
        }

       void TriggerDeathSound(int targetID, bool isDead)
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
