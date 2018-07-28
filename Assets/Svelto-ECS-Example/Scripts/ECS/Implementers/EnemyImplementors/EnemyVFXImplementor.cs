using UnityEngine;

namespace Svelto.ECS.Example.Survive.Characters.Enemies
{
    public class EnemyVFXImplementor : MonoBehaviour, IImplementor, 
        IEnemyVFXComponent
    {
        void Awake ()
        {
            // Setting up the references.
            particle = GetComponentInChildren <ParticleSystem> ();
            play = new DispatchOnSet<bool>(gameObject.GetInstanceID());
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
