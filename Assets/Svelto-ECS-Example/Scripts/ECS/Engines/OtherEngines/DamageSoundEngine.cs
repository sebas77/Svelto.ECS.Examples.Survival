namespace Svelto.ECS.Example.Survive.Sound
{
    public class DamageSoundEngine : IQueryingEntityViewEngine, IStep<DamageInfo, DamageCondition>
    {
        public IEntityDB EntityDb { set; private get; }

        public void Ready()
        {}

        void TriggerDeathSound(EGID targetID)
        {
            uint index;
            EntityDb.QueryEntitiesAndIndex<DamageSoundEntityView>(targetID, out index)[index].audioComponent.playOneShot = AudioType.death;
        }

        void TriggerDamageAudio(EGID sender)
        {
            uint index;
            EntityDb.QueryEntitiesAndIndex<DamageSoundEntityView>(sender, out index)[index].audioComponent.playOneShot = AudioType.damage;
        }

        public void Step(ref DamageInfo token, DamageCondition condition)
        {
            if (condition == DamageCondition.Damage)
                TriggerDamageAudio(token.entityDamagedID);
            else
                TriggerDeathSound(token.entityDamagedID);
        }
    }
}
