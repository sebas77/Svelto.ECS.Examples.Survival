using Svelto.ECS.Example.Survive.Components.Enemies;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.Enemies
{
    public class EnemyVFXImplementor : MonoBehaviour, IImplementor, 
        IEnemyVFXComponent
    {
        public ParticleSystem hitParticles { get { return particle; } }

        void Awake ()
        {
            // Setting up the references.
            particle = GetComponentInChildren <ParticleSystem> ();
            play = new DispatchOnSet<bool>(this.gameObject.GetInstanceID());
            play.NotifyOnValueSet(Play);
        }

        void Play(int sender, bool value)
        {
            if (value == true) particle.Play();
        }

        public ParticleSystem  particle;         // Reference to the particle system that plays when the enemy is damaged.
        public Vector3 position
        {
            set { particle.transform.position = value; }
        }
        public DispatchOnSet<bool> play { get; set; }
    }
}
