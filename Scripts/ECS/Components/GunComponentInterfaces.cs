using UnityEngine;

namespace GunComponents
{
	public interface IGunComponent
    {
        float   timeBetweenBullets { get; }
        Ray     shootRay { get; }
        float   range { get; }
        int     damagePerShot { get; }
        float   timer { get; set; }
        Vector3 lastTargetPosition { get; set; }
        
        Dispatcher<IGunComponent, bool> targetHit { get; }
    }

    public interface IGunFXComponent
    {
        ParticleSystem  particles { get; }
        LineRenderer    line { get; }
        AudioSource     audio { get; }
        Light           light { get; }
        float           effectsDisplayTime { get; }
    }
}
