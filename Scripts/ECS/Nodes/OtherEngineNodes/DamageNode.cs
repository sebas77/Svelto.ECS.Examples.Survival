using SharedComponents;
using Svelto.ES;
using UnityEngine;

namespace SharedEngines
{
    public class DamageNode: INodeWithReferenceID<GameObject>
    {
        public IDamageEventComponent    damageEventComponent;
        public IHealthComponent         healthComponent;

        public DamageNode(GameObject ID) { this.ID = ID; }
        public GameObject ID { get; private set; }
    }
}
