using Soundengines;
using Svelto.ES;

namespace GUIEngines
{
    public class DamageSoundNodeHolder : UnityNodeHolder<DamageSoundNode>
    {
        protected override DamageSoundNode GenerateNode()
        {
            return new DamageSoundNode();
        }
    }
}
