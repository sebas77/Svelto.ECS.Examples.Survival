namespace Svelto.ECS.Example.Survive
{
    public class HealthEngine : IQueryingEntityViewEngine, IStep<DamageInfo>
    {
        public void Ready()
        { }

        public HealthEngine(ISequencer damageSequence)
        {
            _damageSequence = damageSequence;
        }

        public IEntityViewsDB entityViewsDB { set; private get; }

        public void Step(ref DamageInfo damage, int condition)
        {
            var entityView      = entityViewsDB.QueryEntityView<HealthEntityView>(damage.entityDamagedID);
            var healthComponent = entityView.healthComponent;

            healthComponent.currentHealth -= damage.damagePerShot;

            //the HealthEngine can branch the sequencer flow triggering two different
            //conditions
            if (healthComponent.currentHealth <= 0)
                _damageSequence.Next(this, ref damage, DamageCondition.dead);
            else
                _damageSequence.Next(this, ref damage, DamageCondition.damage);
        }

        readonly ISequencer  _damageSequence;
    }
}
