using System;
using Svelto.ES;
using SharedComponents;
using System.Collections.Generic;
using UnityEngine;

namespace Soundengines
{
    public class DamageSoundEngine : INodeEngine
    {
        public Type[] AcceptedNodes()
        {
            return _acceptedNodes;
        }

        public void Add(INode node)
        {
            var healthComponent = (node as DamageSoundNode).healthComponent;

            healthComponent.isDead.observers += TriggerDeathSound;
            healthComponent.isDamaged.observers += TriggerDamageAudio;

            _damageSoundNodes[healthComponent] = (node as DamageSoundNode).audioComponent;
        }

        public void Remove(INode node)
        {
            var healthComponent = (node as DamageSoundNode).healthComponent;

            healthComponent.isDead.observers -= TriggerDeathSound;
            healthComponent.isDamaged.observers -= TriggerDamageAudio;

             _damageSoundNodes.Remove(healthComponent);
        }

       void TriggerDeathSound(IHealthComponent sender, GameObject target)
       {
            var playerAudio =  _damageSoundNodes[sender];

            playerAudio.audioSource.PlayOneShot(playerAudio.death);
       }
    
        void TriggerDamageAudio(IHealthComponent sender, DamageInfo isDamaged)
        {
            var playerAudio = _damageSoundNodes[sender];

            playerAudio.audioSource.PlayOneShot(playerAudio.damage);
        }

        Type[] _acceptedNodes = new Type[1] { typeof(DamageSoundNode) };

        Dictionary<IHealthComponent, IDamageSoundComponent>  _damageSoundNodes = new Dictionary<IHealthComponent, IDamageSoundComponent>();
    }
}
