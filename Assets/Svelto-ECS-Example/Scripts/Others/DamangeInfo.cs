using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
    public struct DamageInfo
    {
        public int damagePerShot { get; }
        public Vector3 damagePoint { get; }
        public EntityDamagedType entityType { get; }
        
        public DamageInfo(int damage, Vector3 point, EntityDamagedType edt) : this()
        {
            damagePerShot = damage;
            damagePoint = point;
            entityType = edt;
        }
    }

    public enum EntityDamagedType
    {
        Player,
        Enemy
    }
}
    
