using Svelto.ES;
using UnityEngine;

namespace Components.Gun
{
	public interface IGunComponent: IComponent
    {
        float   timeBetweenBullets { get; }
        Ray     shootRay { get; }
        float   range { get; }
        int     damagePerShot { get; }
        float   timer { get; set; }
        Vector3 lastTargetPosition { get; set; }

        DispatcherOnSet<int, bool> targetHit { get; }
    }

    public interface IGunFXComponent: IComponent
    {
        ParticleSystem  particles { get; }
        LineRenderer    line { get; }
        AudioSource     audio { get; }
        Light           light { get; }
        float           effectsDisplayTime { get; }
    }
}
