using Svelto.ECS.Example.Survive.Camera;
using Svelto.ECS.Example.Survive.Sound;

namespace Svelto.ECS.Example.Survive.Characters.Player
{
	public class PlayerEntityDescriptor : IEntityDescriptor
	{
		static readonly IEntityBuilder[] _entitiesToBuild =
		{
			new EntityBuilder<PlayerEntityViewStruct>(),
			new EntityBuilder<TargetEntityViewStruct>(),
			new EntityBuilder<DamageSoundEntityView>(),
			new EntityBuilder<CameraTargetEntityView>(),
			new EntityBuilder<HealthEntityStruct>(),
			new EntityBuilder<PlayerInputDataStruct>()
		};

		public IEntityBuilder[] entitiesToBuild
		{
			get { return _entitiesToBuild; }
		}
	}
}
