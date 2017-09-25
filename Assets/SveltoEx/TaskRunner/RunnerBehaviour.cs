#if UNITY_5 || UNITY_5_3_OR_NEWER
using System.Collections;
using UnityEngine;

namespace Svelto.Tasks.Internal
{
    class RunnerBehaviour : MonoBehaviour
    {
        new public void StartCoroutineN(IEnumerator enumerator)
        {
            _mainRoutine = enumerator;
        }

        void Update()
        {
            if (_mainRoutine != null)
                _mainRoutine.MoveNext();
        }

        IEnumerator _mainRoutine;
    }

    class RunnerBehaviourPhysic : MonoBehaviour
    {
        void FixedUpdate()
        {
            if (_mainFixedRoutine != null)
                 _mainFixedRoutine.MoveNext();
        }

        public void StartPhysicCoroutine(IEnumerator enumerator)
        {
            _mainFixedRoutine = enumerator;
        }

        IEnumerator _mainFixedRoutine;

    }
}
#endif
