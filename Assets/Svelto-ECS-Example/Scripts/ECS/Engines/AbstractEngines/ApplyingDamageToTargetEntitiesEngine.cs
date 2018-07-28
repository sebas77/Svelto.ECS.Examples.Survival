using System.Collections;
using Svelto.ECS.Example.Survive.Characters;
using Svelto.ECS.Example.Survive.Characters.Player;

namespace Svelto.ECS.Example.Survive
{
    /// <summary>
    ///
    /// The responsability of this engine is to apply the damage to any
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

        IEnumerator CheckEnergy()
        {
            while (true)
            {
                int count;
                var entities = entitiesDB.QueryEntities<TargetEntityViewStruct>(out count);
                var healths = entitiesDB.QueryEntities<HealthEntityStruct>(out count);

                for (int i = 0; i < count; i++)
                {
                    if (entities[i].damageInfo.damagePerShot > 0)
                    {
                        healths[i].currentHealth -= entities[i].damageInfo.damagePerShot;
                        entities[i].damaged = true;
                    }
                    else
                        entities[i].damaged = false;
                }
            }
        }

        public IEntitiesDB entitiesDB { set; private get; }
    }
}
