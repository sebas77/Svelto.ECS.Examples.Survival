using System;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.Enemies
{
    public interface IEnemyTargetComponent: IComponent
    {
        bool targetInRange { get; }
    }
    
    public interface IEnemySinkComponent : IComponent
    {
        float sinkAnimSpeed { get; }
    }

    public interface IEnemyAttackDataComponent: IComponent
    {
        int   damage         { get; }
        float attackInterval { get; }
        float timer          { get; set; }
    }

    public interface IEnemyMovementComponent: IComponent
    {
        bool navMeshEnabled {  set; }
        Vector3 navMeshDestination { set; }
        bool setCapsuleAsTrigger { set; }
    }

    public interface IEnemyTriggerComponent: IComponent
    {
        event Action<int, int, bool> entityInRange;

        bool targetInRange { set; }
    }

    public interface IEnemyVFXComponent: IComponent
    {
        Vector3             position { set; }
        DispatchOnSet<bool> play     { get; }
    }
}