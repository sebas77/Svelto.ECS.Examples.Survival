using UnityEngine;
using Svelto.ECS.Example.Survive.Nodes.Enemies;
using Svelto.DataStructures;
using System;
using Svelto.ECS.Example.Survive.Components.Enemy;

namespace Svelto.ECS.Example.Survive.EntityDescriptors.EnemySpawner
{
    class EnemySpawnerEntityDescriptor : EntityDescriptor
     {
         IEnemySpawnerComponent[] _components;

        public EnemySpawnerEntityDescriptor(IEnemySpawnerComponent[] componentsImplementor):base(null, null)
		{
             _components = componentsImplementor;
        }

        //this shows how you can override the BuildNodes to adapt it to your needs. Without calling the base
        //function, the automatic component injection will be disabled
        public override FasterList<INode> BuildNodes(int ID)
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

