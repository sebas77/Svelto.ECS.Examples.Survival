using Svelto.ES;
using UnityEngine;

namespace Components.Damageable
{
    public interface IHealthComponent: IComponent
    {
        int             currentHealth   { get; set; }
        bool            hasBeenDamaged  { get; set; }

        Dispatcher<int>             isDead          { get; }
        Dispatcher<int, DamageInfo> isDamaged       { get; }
    }

    public interface IDamageEventComponent: IComponent
    {
        Dispatcher<int, DamageInfo>    damageReceived  { get; }
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
