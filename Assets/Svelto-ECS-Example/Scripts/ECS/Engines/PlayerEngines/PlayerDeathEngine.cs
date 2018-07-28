using System.Collections;
using Svelto.ECS.Example.Survive.HUD;

namespace Svelto.ECS.Example.Survive.Characters.Player
{
    public class PlayerDeathEngine:IQueryingEntitiesEngine
    {
        public PlayerDeathEngine(PlayerDeathSequencer playerDeathSequence)
        {
            _playerDeathSequence = playerDeathSequence;
        }

        public void Ready()
        {
            CheckEnergy().Run();
        }

        IEnumerator CheckEnergy()
        {
            while (true)
            {
                int count;
                var healths  = entitiesDB.QueryEntities<HealthEntityStruct>(out count);

                for (int i = 0; i < count; i++)
                {
                    if (healths[i].currentHealth <= 0)
                        _playerDeathSequence.Next(this);
                }

                yield return null;
            }
        }

        public IEntitiesDB entitiesDB { set; private get; }

        readonly PlayerDeathSequencer _playerDeathSequence;
    }
}