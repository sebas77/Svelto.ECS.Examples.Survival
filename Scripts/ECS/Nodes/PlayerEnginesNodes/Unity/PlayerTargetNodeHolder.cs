using PlayerEngines;
using Svelto.ES;

namespace PlayerEngines
{
    public class PlayerTargetNodeHolder : UnityNodeHolder<PlayerTargetNode>
    {
        protected override PlayerTargetNode GenerateNode()
        {
            return new PlayerTargetNode(gameObject);
        }
    }
}
