using System.Collections;

namespace Svelto.ECS.Example.Survive.Characters
{
    /// <summary>
    ///
    /// The responsibility of this engine is to apply the damage to any
    /// damageable entity. If the logic applied to the enemy was different
    /// than the logic applied to the player, I would have created two
    /// different engines
    /// 
    /// </summary>
    public class ApplyingDamageToTargetEntitiesEngine : IQueryingEntitiesEngine
    {
        public void Ready()
        {
            CheckEnergy().Run();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        IEnumerator CheckEnergy()
        {
            while (true)
            {
                int count;
                var entities = entitiesDB.QueryEntities<DamageableEntityStruct>(out count);
                var healths = entitiesDB.QueryEntities<HealthEntityStruct>(out count);

                for (int i = 0; i < count; i++)
                {
                    if (entities[i].damageInfo.damagePerShot > 0)
                    {
                        healths[i].currentHealth -= entities[i].damageInfo.damagePerShot;
                        entities[i].damageInfo.damagePerShot = 0;
                        entities[i].damaged = true;
                    }
                    else
                        entities[i].damaged = false;
                }

                yield return null;
            }
        }

        public IEntitiesDB entitiesDB { set; private get; }
    }
}
