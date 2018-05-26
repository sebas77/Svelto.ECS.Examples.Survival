using Svelto.ECS.Example.Survive.Camera;
using Svelto.ECS.Example.Survive.Enemies;
using Svelto.ECS.Example.Survive.Sound;

namespace Svelto.ECS.Example.Survive.Player
{
	public class PlayerEntityDescriptor : GenericEntityDescriptor<PlayerEntityView,
		EnemyTargetEntityView, DamageSoundEntityView, CameraTargetEntityView, HealthEntityStruct>
	{
		
	}
}
