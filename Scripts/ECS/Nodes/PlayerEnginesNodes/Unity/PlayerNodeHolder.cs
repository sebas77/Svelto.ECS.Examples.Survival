using Svelto.ES;

namespace PlayerEngines
{
    public class PlayerNodeHolder : UnityNodeHolder<PlayerNode>
    {
        protected override PlayerNode GenerateNode()
        {
            return new PlayerNode();
        }
    }
}
