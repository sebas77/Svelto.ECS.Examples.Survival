using UnityEngine;

public interface IRayCaster
{
    int CheckHit(Ray ray, float range, int layer, int mask, out Vector3 point);
    int CheckHit(Ray ray, float range, int mask, out Vector3 point);
}

public class RayCaster : IRayCaster
{
        public int CheckHit(Ray ray, float range, int layer, int mask, out Vector3 point)
        {
            RaycastHit shootHit;
            Physics.Raycast(ray, 
                out shootHit, range, mask);

            point = shootHit.point;
            if (shootHit.collider != null)
            {
                var colliderGameObject = shootHit.collider.gameObject;
                if (colliderGameObject.layer == layer)
                    return colliderGameObject.GetInstanceID();
            }

            return -1;
        }        
        
        public int CheckHit(Ray ray, float range, int mask, out Vector3 point)
        {
            RaycastHit shootHit;
            Physics.Raycast(ray, 
                out shootHit, range, mask);

            point = shootHit.point;
            if (shootHit.collider != null)
            {
                var colliderGameObject = shootHit.collider.gameObject;

                return colliderGameObject.GetInstanceID();
            }

            return -1;
        }        
    }
