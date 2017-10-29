
using UnityEngine;

public class UnityWay : MonoBehaviour
{
    private void Start()
    {
        _realTarget.Set(1, 2, 3);
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            var x = (_realTarget.x - _position.x);
            var y = (_realTarget.y - _position.y);
            var z = (_realTarget.z - _position.z);

            var sqrdmagnitude = x * x + y * y + z * z;
            _position.x = x * sqrdmagnitude;
            _position.y = y * sqrdmagnitude;
            _position.z = z * sqrdmagnitude;
        }
    }

    Vector3 _position, _realTarget;
}

