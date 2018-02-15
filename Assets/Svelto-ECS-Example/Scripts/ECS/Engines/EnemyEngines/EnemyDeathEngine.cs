namespace Svelto.ECS.Example.Survive.Enemies
{
    public class EnemyDeathEngine:IStep<DamageInfo>, IEngine
    {
        public EnemyDeathEngine(IEntityFunctions entityFunctions)
        {
            _entityFunctions = entityFunctions;
        }
        
        public void Step(ref DamageInfo token, int condition)
        {
            _entityFunctions.RemoveEntity<EnemyEntityDescriptor>(token.entityDamagedID);
        }

        readonly IEntityFunctions _entityFunctions;
    }
}