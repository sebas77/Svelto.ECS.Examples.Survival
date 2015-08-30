using Svelto.ES;

namespace SharedEngines
{
    public class DamageNodeHolder : UnityNodeHolder<DamageNode>
    {
        protected override DamageNode GenerateNode()
        {
            return new DamageNode(this.gameObject);
        }
    }
}
