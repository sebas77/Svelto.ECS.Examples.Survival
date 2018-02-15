namespace Svelto.ECS.Example.Survive.Sound
{
    public class DamageSoundEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {}

        void TriggerDeathSound(int targetID)
        {
            var audioEntityView =  entityViewsDB.QueryEntityView<DamageSoundEntityView>(targetID);
            var audioComponent = audioEntityView.audioComponent;

            audioComponent.playOneShot = AudioType.death;
        }

        void TriggerDamageAudio(int sender)
        {
           var audioEntityView = entityViewsDB.QueryEntityView<DamageSoundEntityView>(sender);
           var audioComponent = audioEntityView.audioComponent;

           audioComponent.playOneShot = AudioType.damage;
        }

        public void Step(ref DamageInfo token, int condition)
        {
            if (condition == DamageCondition.damage)
                TriggerDamageAudio(token.entityDamagedID);
            else
                TriggerDeathSound(token.entityDamagedID);
        }
    }
}
