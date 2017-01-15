using UnityEngine;
using Svelto.ECS.Example.Nodes.Enemies;
using Svelto.DataStructures;
using System;
using Svelto.ECS.Example.Components.Enemy;

namespace Svelto.ECS.Example.EntityDescriptors.EnemySpawner
{
    class EnemySpawnerEntityDescriptor : EntityDescriptor
     {
         IEnemySpawnerComponent[] _components;

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

	[DisallowMultipleComponent]
	public class EnemySpawnerEntityDescriptorHolder:MonoBehaviour, IEntityDescriptorHolder
	{
		public EntityDescriptor BuildDescriptorType(object[] extraImplentors = null)
		{
			return new EnemySpawnerEntityDescriptor(GetComponents<IEnemySpawnerComponent>());
		}
	}
}

