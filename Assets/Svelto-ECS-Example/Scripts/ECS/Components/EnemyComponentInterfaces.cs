using System;
using UnityEngine;
using UnityEngine.AI;

namespace Svelto.ECS.Example.Survive.Components.Enemies
{
    public interface IEnemyAttackComponent: IComponent
    {
        bool targetInRange { get; }
    }

    public interface IEnemyAttackDataComponent: IComponent
    {
        int   damage            { get; }
        float attackInterval    { get; }
        float timer             { get; set; }
    }

    public interface IEnemyMovementComponent: IComponent
    {
        NavMeshAgent navMesh            { get; }
        float sinkSpeed                 { get; }
        CapsuleCollider capsuleCollider { get; }
    }

    public interface IEnemyTriggerComponent: IComponent
    {
        event Action<int, int, bool> entityInRange;

        bool targetInRange { set; }
    }

    public interface IEnemyVFXComponent: IComponent
    {
        ParticleSystem hitParticles { get; }
    }
}
