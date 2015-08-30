using EnemyEngines;
using Svelto.ES;

namespace EnemyEngines
{
    public class EnemyTargetNodeHolder : UnityNodeHolder<EnemyTargetNode>
    {
        protected override EnemyTargetNode GenerateNode()
        {
            return new EnemyTargetNode(gameObject);
        }
    }
}
