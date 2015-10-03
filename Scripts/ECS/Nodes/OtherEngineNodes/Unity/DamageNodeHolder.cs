using Svelto.ES;

namespace HealthEngines
{
    public class DamageNodeHolder : UnityNodeHolder<DamageNode>
    {
        protected override DamageNode GenerateNode()
        {
            return new DamageNode(this.gameObject);
        }
    }
}
