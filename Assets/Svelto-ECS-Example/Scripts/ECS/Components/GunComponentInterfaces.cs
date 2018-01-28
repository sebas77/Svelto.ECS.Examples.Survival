using UnityEngine;

namespace Svelto.ECS.Example.Survive.Components.Gun
{
	public interface IGunAttributesComponent: IComponent
    {
        float   timeBetweenBullets { get; }
        Ray     shootRay { get; }
        float   range { get; }
        int     damagePerShot { get; }
        float   timer { get; set; }
        Vector3 lastTargetPosition { get; set; }
    }

    public interface IGunHitTargetComponent : IComponent
    {
        DispatchOnSet<bool> targetHit { get; }
    }

    //Big question here: should a component return anything else
    //than a valuetype? the answer is no! However I think exceptions
    //can be made when implementors are used as bridge between the
    //platform and Svelto.ECS. As long as the functions used are 
    //mockable, it should be all right. HOWEVER NEVER hold a reference
    //of a user class instance. All the logic you write
    //must be inside an engine.
    //However if the platform (Unity in this case) function to
    //use is a setter or getter, then it's way better to
    //declare directly that setter or getter instead
    //to return the a reference to an object.
    //Check IAnimationComponent and IRigidBodyComponent for
    //examples
    public interface IGunFXComponent: IComponent
    {
        ParticleSystem  particles { get; }
        LineRenderer    line { get; }
        AudioSource     audio { get; }
        Light           light { get; }
        float           effectsDisplayTime { get; }
    }
}
