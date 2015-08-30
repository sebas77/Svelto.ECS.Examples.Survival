using Svelto.ES;
using EnemyComponents;

namespace EnemyEngines
{
    public class EnemySpawningNodeHolder : BaseNodeHolder<EnemySpawningNode>, INodeHolder
    {
        override protected EnemySpawningNode ReturnNode()
        {
            var node = new EnemySpawningNode(gameObject);

            node.spawnerComponents = gameObject.GetComponentsInChildren<IEnemySpawnerComponent>(true);

            return node;
        }
    }
}
