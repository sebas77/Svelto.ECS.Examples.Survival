using Svelto.ES;

namespace PlayerEngines
{
    public class PlayerGunNodeHolder : UnityNodeHolder<PlayerGunNode>
    {
        protected override PlayerGunNode GenerateNode()
        {
            return new PlayerGunNode();
        }
    }
}
