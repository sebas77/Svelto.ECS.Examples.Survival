using SharedComponents;
using Svelto.ES;

namespace Soundengines
{
    public class DamageSoundNode: INode
    {
        public IDamageSoundComponent    audioComponent;
        public IHealthComponent         healthComponent;
    }
}
