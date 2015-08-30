using UnityEngine;

namespace SharedComponents
{
    public interface IHealthComponent
    {
        int             currentHealth   { get; set; }
        bool            hasBeenDamaged  { get; set; }
        
        Dispatcher<IHealthComponent, GameObject>   isDead          { get; }
        Dispatcher<IHealthComponent, DamageInfo>   isDamaged       { get; }
    }

    public interface IDamageEventComponent
    {
        Dispatcher<IDamageEventComponent, DamageInfo>    damageReceived  { get; }
    }

    public struct DamageInfo
    {
        public readonly int         damagePerShot;
        public readonly Vector3     damagePoint;
        
        public DamageInfo(int damage, Vector3 point) : this()
        {
            damagePerShot = damage;
            damagePoint = point;
        }
    }
}
