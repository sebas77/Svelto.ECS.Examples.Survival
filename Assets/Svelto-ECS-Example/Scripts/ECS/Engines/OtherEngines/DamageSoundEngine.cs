using System.Collections;
using Svelto.ECS.Example.Survive.Characters.Player;

namespace Svelto.ECS.Example.Survive.Characters.Sounds
{
    public class DamageSoundEngine : IQueryingEntitiesEngine, IStep<PlayerDeathCondition>, IStep<EnemyDeathCondition>
    {
        public IEntitiesDB entitiesDB { set; private get; }

        public void Ready()
        {
            CheckForDamage().Run();
        }

        void TriggerDeathSound(EGID targetID)
        {
            uint index;
            entitiesDB.QueryEntitiesAndIndex<DamageSoundEntityView>(targetID,
                                                                    out index)[index]
                      .audioComponent.playOneShot = AudioType.death;
        }

        IEnumerator CheckForDamage()
        {
            while (true)
            {
                int count;
                var damageableEntities = entitiesDB.QueryEntities<DamageableEntityStruct>(out count);

                for (int i = 0; i < count; i++)
                {
                    uint index;
                    
                    if (damageableEntities[i].damaged == false) continue;

                    entitiesDB.QueryEntitiesAndIndex<DamageSoundEntityView>(damageableEntities[i].ID,
                                                                            out index)[index]
                              .audioComponent.playOneShot = AudioType.damage;
                }

                yield return null;
            }
        }

        public void Step(PlayerDeathCondition condition, EGID id)
        {
            TriggerDeathSound(id);
        }

        public void Step(EnemyDeathCondition condition, EGID id)
        {
            TriggerDeathSound(id);
        }
    }
}
