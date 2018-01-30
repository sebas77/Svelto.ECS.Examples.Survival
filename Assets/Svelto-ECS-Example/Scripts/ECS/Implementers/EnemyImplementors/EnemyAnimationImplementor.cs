using Svelto.ECS.Example.Survive.Components.Base;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Implementors.Enemies
{
    public class EnemyAnimationImplementor : MonoBehaviour, IImplementor, 
        IAnimationComponent, IEnemySinkComponent
    {
        public float SinkSpeed = 2.5f; // The speed at which the enemy sinks through the floor when dead.
        
        public void setBool(string name, bool value)
        {
            _anim.SetBool(name, value);
        }

        public string trigger { set { _anim.SetTrigger(value);} }
        public float sinkAnimSpeed { get { return SinkSpeed; } }

        void Awake ()
        {
            // Setting up the references.
            _anim = GetComponent <Animator> ();
        }
        
        Animator        _anim;                 // Reference to the animator.
    }
}
