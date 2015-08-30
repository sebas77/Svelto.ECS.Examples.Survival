using Svelto.ES;

namespace GUIEngines
{
    public class GUINodeHolder : UnityNodeHolder<GUINode>
    {
        protected override GUINode GenerateNode()
        {
            return new GUINode();
        }
    }
}
