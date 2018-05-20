namespace Svelto.ECS.Example.Survive.Sound
{
    public class DamageSoundEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Ready()
        {}

        void TriggerDeathSound(EGID targetID)
        {
            DamageSoundEntityView audioEntityView;
            entityViewsDB.TryQueryEntityView(targetID, out audioEntityView);
            var audioComponent = audioEntityView.audioComponent;

            audioComponent.playOneShot = AudioType.death;
        }

        void TriggerDamageAudio(EGID sender)
        {
            DamageSoundEntityView audioEntityView;
            entityViewsDB.TryQueryEntityView(sender, out audioEntityView);
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
