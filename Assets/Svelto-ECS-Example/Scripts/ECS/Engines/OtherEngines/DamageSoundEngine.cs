namespace Svelto.ECS.Example.Survive.Sound
{
    public class DamageSoundEngine : IQueryingEntitiesEngine
    {
        public IEntitiesDB entitiesDB { set; private get; }

        public void Ready()
        {}

        void TriggerDeathSound(EGID targetID)
        {
            uint index;
            entitiesDB.QueryEntitiesAndIndex<DamageSoundEntityView>(targetID,
                                                                    out index)[index]
                      .audioComponent.playOneShot = AudioType.death;
        }

        void TriggerDamageAudio(EGID sender)
        {
            uint index;
            entitiesDB.QueryEntitiesAndIndex<DamageSoundEntityView>(sender,
                                                                    out index)[index]
                      .audioComponent.playOneShot = AudioType.damage;
        }

        public void Step()
        {
            //if (condition == DamageCondition.Damage)
                //TriggerDamageAudio(id);
            //else
              //  TriggerDeathSound(id);
        }
    }
}
