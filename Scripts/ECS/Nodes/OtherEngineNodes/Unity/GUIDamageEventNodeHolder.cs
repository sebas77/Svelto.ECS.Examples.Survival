using Svelto.ES;

namespace GUIEngines
{
    public class GUIDamageEventNodeHolder : UnityNodeHolder<GUIDamageEventNode>
    {
        protected override GUIDamageEventNode GenerateNode()
        {
            return new GUIDamageEventNode();
        }
    }
}
