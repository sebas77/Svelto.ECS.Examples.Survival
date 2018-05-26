using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
    public struct DamageInfo
    {
        public int damagePerShot { get; }
        public Vector3 damagePoint { get; }
        public EGID entityDamagedID { get; }
        public EntityDamagedType entityType { get; }
        
        public DamageInfo(int damage, Vector3 point, EGID entity, EntityDamagedType edt) : this()
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
    
