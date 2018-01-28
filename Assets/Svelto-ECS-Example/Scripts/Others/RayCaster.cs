using UnityEngine;

    public class RayCaster
    {
        public int CheckHit(Ray ray, float range, int layer, int mask, out Vector3 point)
        {
            RaycastHit shootHit;
            Physics.Raycast(ray, 
                out shootHit, range, mask);

            point = shootHit.point;
            var colliderGameObject = shootHit.collider.gameObject;
            if (colliderGameObject.layer == layer)
                return colliderGameObject.GetInstanceID();

            return -1;
        }        
    }
