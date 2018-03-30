namespace Svelto.ECS.Example.Survive.Sound
{
    public class DamageSoundEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {}

        void TriggerDeathSound(EGID targetID)
        {
            var audioEntityView =  entityViewsDB.QueryEntityView<DamageSoundEntityView>(targetID);
            var audioComponent = audioEntityView.audioComponent;

            audioComponent.playOneShot = AudioType.death;
        }

        void TriggerDamageAudio(EGID sender)
        {
           var audioEntityView = entityViewsDB.QueryEntityView<DamageSoundEntityView>(sender);
           var audioComponent = audioEntityView.audioComponent;

           audioComponent.playOneShot = AudioType.damage;
        }

        public void Step(ref DamageInfo token, int condition)
        {
            if (condition == DamageCondition.Damage)
                TriggerDamageAudio(token.entityDamagedID);
            else
                TriggerDeathSound(token.entityDamagedID);
        }
    }
}
