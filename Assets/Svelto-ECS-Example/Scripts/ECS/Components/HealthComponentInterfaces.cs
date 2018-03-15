using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
    public interface IHealthComponent : IComponent
    {
        int currentHealth { get; set; }
    }

    public struct DamageInfo
    {
        public int damagePerShot { get; private set; }
        public Vector3 damagePoint { get; private set; }
        public int entityDamagedID { get; private set; }
        public EntityDamagedType entityType  { get; private set; }
        
        public DamageInfo(int damage, Vector3 point, int entity, EntityDamagedType edt) : this()
        {
            damagePerShot = damage;
            damagePoint = point;
            entityDamagedID = entity;
            entityType = edt;
        }
    }

    public enum EntityDamagedType
    {
        Player,
        Enemy
    }
}
    
