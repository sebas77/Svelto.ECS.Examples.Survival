namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerDeathEngine:IEngine, IStep<DamageInfo, DamageCondition>
    {
        public PlayerDeathEngine(IEntityFunctions entityFunctions)
        {
            _entityFunctions = entityFunctions;
        }
        
        public void Step(ref DamageInfo token, DamageCondition condition)
        {
            _entityFunctions.RemoveEntity(token.entityDamagedID);
        }

        readonly IEntityFunctions _entityFunctions;
    }
}