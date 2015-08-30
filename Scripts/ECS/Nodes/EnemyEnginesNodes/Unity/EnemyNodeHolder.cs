using EnemyEngines;
using Svelto.ES;

namespace EnemyEngines
{
    public class EnemyNodeHolder : UnityNodeHolder<EnemyNode>
    {
        protected override EnemyNode GenerateNode()
        {
            return new EnemyNode(gameObject);
        }
    }
}
