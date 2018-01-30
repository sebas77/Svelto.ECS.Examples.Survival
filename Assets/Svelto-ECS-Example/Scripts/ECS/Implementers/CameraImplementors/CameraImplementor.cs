using Svelto.ECS.Example.Survive.Components.Base;
using UnityEngine;

namespace Svelto.ECS.Example.Survive.CameraImplementors
{
    public class CameraImplementor : MonoBehaviour, ITransformComponent
    {
        Transform cameraTransform;

        void Awake()
        {
            cameraTransform = this.transform;
        }

        public Vector3 position
        {
            get { return cameraTransform.position; }
            set { cameraTransform.position = value; }
        }

        public Quaternion rotation
        {
            set { cameraTransform.rotation = value; }
        }
    }
}