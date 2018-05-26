using Svelto.ECS.Example.Survive.Player;

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
            entityViewsDB.ExecuteOnEntity(damage.entityDamagedID, ref damage,
                                          (ref HealthEntityStruct healthEntityStruct,
                                           ref DamageInfo         damageInfo) =>
                                          {
                                              healthEntityStruct.currentHealth -=
                                                  damageInfo.damagePerShot;
                                                  
                                              //the HealthEngine can branch the sequencer flow triggering two different
                                              //conditions
                                              if (healthEntityStruct.currentHealth <= 0)
                                                  _damageSequence.Next(this, ref damageInfo,
                                                                       DamageCondition.Dead);
                                              else
                                                  _damageSequence.Next(this, ref damageInfo,
                                                                       DamageCondition
                                                                          .Damage);
                                          }
                                         );
            
        }

        readonly ISequencer  _damageSequence;
    }
}
