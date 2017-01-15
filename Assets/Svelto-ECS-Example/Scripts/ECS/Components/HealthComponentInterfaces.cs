using System;
using UnityEngine;

namespace Svelto.ECS.Example.Components.Damageable
{
    public interface IHealthComponent : IComponent
    {
        int currentHealth { get; set; }
    }

    public struct DamageInfo: IDamageInfo
    {
        public int damagePerShot { get; private set; }
        public Vector3 damagePoint { get; private set; }
        public int entityDamaged { get; private set; }
        
        public DamageInfo(int damage, Vector3 point, int entity) : this()
        {
            damagePerShot = damage;
            damagePoint = point;
            entityDamaged = entity;
        }
    }

    public struct PlayerDamageInfo: IDamageInfo
    {
        public int damagePerShot { get; private set; }
        public Vector3 damagePoint { get; private set; }
        public int entityDamaged { get; private set; }
        
        public PlayerDamageInfo(int damage, Vector3 point, int entity) : this()
        {
            damagePerShot = damage;
            damagePoint = point;
            entityDamaged = entity;
        }
    }

    public interface IDamageInfo
    {
        int damagePerShot { get; }
        Vector3 damagePoint { get; }
        int entityDamaged { get; }
    }
}
    
