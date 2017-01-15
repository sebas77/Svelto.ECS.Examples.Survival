using UnityEngine;

namespace Svelto.ECS.Example.Components.Damageable
{
    public interface IHealthComponent: IComponent
    {
        int             currentHealth   { get; set; }
        //bool            hasBeenDamaged  { get; set; }

        DispatcherOnChange<bool>           isDead         { get; }
        Legacy.Dispatcher<int, DamageInfo> isDamaged      { get; }
    }

    public interface IDamageEventComponent: IComponent
    {
        Svelto.ECS.Legacy.Dispatcher<int, DamageInfo>    damageReceived  { get; }
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
