using Svelto.ECS.Example.Survive.Components.Base;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.Enemies
{
    public class EnemyAudioImplementor : MonoBehaviour, IImplementor, 
        IDamageSoundComponent
    {
        public AudioClip deathClip;                 // The sound to play when the enemy dies.
        public AudioClip damageClip;                 // The sound to play when the enemy dies.

        public AudioSource audioSource { get { return _enemyAudio; } }
        public AudioClip death { get { return deathClip; } }
        public AudioClip damage { get { return damageClip; } }

        void Awake ()
        {// Setting up the references.
            _enemyAudio = GetComponent <AudioSource> ();
        }

        AudioSource     _enemyAudio;           // Reference to the audio source.
    }
}
