namespace Svelto.ECS.Example.Survive.Player
{
    public class PlayerDeathEngine:IStep<DamageInfo>, IEngine
    {
        public PlayerDeathEngine(IEntityFunctions entityFunctions)
        {
            _entityFunctions = entityFunctions;
        }
        
        public void Step(ref DamageInfo token, int condition)
        {
            _entityFunctions.RemoveEntity<PlayerEntityDescriptor>(token.entityDamagedID);
        }

        readonly IEntityFunctions _entityFunctions;
    }
}