using UnityEngine;

namespace Svelto.ECS.Example.Survive
{
    public struct DamageInfo
    {
        public int damagePerShot;
        public Vector3 damagePoint { get; }
        
        public DamageInfo(int damage, Vector3 point) : this()
        {
            damagePerShot = damage;
            damagePoint = point;
        }
    }
}
    
