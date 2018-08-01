using System.Collections;

namespace Svelto.ECS.Example.Survive.Characters
{
    public class DeathEngine:IQueryingEntitiesEngine
    {
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
                        healths[i].dead = true;
                }

                yield return null;
            }
        }

        public IEntitiesDB entitiesDB { set; private get; }
    }
}