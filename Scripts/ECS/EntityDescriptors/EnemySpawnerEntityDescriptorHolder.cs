using Svelto.ES;
using UnityEngine;
using Nodes.Enemies;
using Svelto.DataStructures;
using System;
using Components.Enemy;

namespace EntityDescriptors.EnemySpawner
{
	[DisallowMultipleComponent]
	public class EnemySpawnerEntityDescriptorHolder:MonoBehaviour, IEntityDescriptorHolder
	{
		class EnemySpawnerEntityDescriptor : EntityDescriptor
        {
            private IEnemySpawnerComponent[] _components;

            public EnemySpawnerEntityDescriptor(IEnemySpawnerComponent[] componentsImplementor):base(null, componentsImplementor)
			{
                _components = componentsImplementor;
            }

            public override FasterList<INode> BuildNodes(int ID, Action<INode> removeAction)
            {
                var nodes = new FasterList<INode>();

                var node = new EnemySpawningNode
                {
                    spawnerComponents = _components
                };

                nodes.Add(node);

                return nodes;
            }
        }

		EntityDescriptor IEntityDescriptorHolder.BuildDescriptorType()
		{
			return new EnemySpawnerEntityDescriptor(GetComponents<IEnemySpawnerComponent>());
		}
	}
}

