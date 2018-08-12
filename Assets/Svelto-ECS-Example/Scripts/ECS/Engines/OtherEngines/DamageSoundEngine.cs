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
                entitiesDB.ExecuteOnAllEntities((ref DamageableEntityStruct damagedEntity,
                                                 IEntitiesDB            entitiesDB_) =>
                {
                    uint index;
                    
                    if (damagedEntity.damaged == false) return;

                    //This is when things get tricky. When iterating on an entity view, you can't 
                    //ever assume that the index used during the iteration can be used as index
                    //for another array of entity views, unless groups are used.
                    //Since in this case we are iterating over ALL the damageableEntityStructs regardless
                    //the group (enemies or player), I can't even know the index to use.
                    //through the ID of the entity, I can know the group where the current entity is
                    //but the only way to know the index of the entity in the array of enties is 
                    //trhough mapping. The EntityID is so mapped to the correct group and index.
                    entitiesDB_.QueryEntitiesAndIndex<DamageSoundEntityView>(damagedEntity.ID,
                                                                            out index)[index]
                              .audioComponent.playOneShot = AudioType.damage;
                });
                
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
