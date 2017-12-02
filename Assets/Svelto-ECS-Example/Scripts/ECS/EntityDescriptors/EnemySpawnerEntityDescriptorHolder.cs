using UnityEngine;
using Svelto.ECS.Example.Survive.Nodes.Enemies;
using Svelto.ECS.Example.Survive.Others;
using Svelto.ECS.Example.Survive.Implementors.Enemies;

namespace Svelto.ECS.Example.Survive.EntityDescriptors.EnemySpawner
{
	class EnemySpawnerEntityDescriptor : GenericEntityDescriptor<EnemySpawningNode>
	{
		public EnemySpawnerEntityDescriptor(object[] objects):base(objects)
		{}
	}

	[DisallowMultipleComponent]
	public class EnemySpawnerEntityDescriptorHolder:MonoBehaviour, IEntityDescriptorHolder
	{
		public EntityDescriptor BuildDescriptorType(object[] extraImplentors = null)
		{
			var enemySpawners = GetComponents<EnemySpawnDataSource>();
				
			return new EnemySpawnerEntityDescriptor(new object[] {new EnemySpawningImplementor(enemySpawners)});
		}
	}
}

