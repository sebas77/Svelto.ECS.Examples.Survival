#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_IPHONE || UNITY_ANDROID || UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System;

namespace Svelto.Tasks.Internal
{
	internal class MonoRunner: IRunner
	{
		RunnerBehaviour 	_component;

        public bool paused { set; get; }
        public bool stopped { private set; get; }
				
		void Init()
		{
			GameObject go = new GameObject("TaskRunner");

			go.hideFlags = HideFlags.HideInHierarchy;

			if ((_component = go.GetComponent<RunnerBehaviour>()) == null)
				_component = go.AddComponent<RunnerBehaviour>();

			paused = false;
			stopped = false;
		}

        public void StopAllCoroutines()
		{
            StopManagedCoroutines();

            _component.StopAllCoroutines();
		}

        public void StopManagedCoroutines()
        {
            stopped = true;
        }

		public void StartCoroutine(IEnumerator task)
		{
            if (RunnerBehaviour.isQuitting == true)
                return;

			stopped = false;
			paused = false;

            if (_component == null)
                Init();

            _component.gameObject.SetActive(true);
			_component.enabled = true;

			_component.StartCoroutine(StartCoroutineInternal(task));
		}

        IEnumerator StartCoroutineInternal(IEnumerator coroutine)
        {
            while (true)
            {
                try
                {
                    if (!coroutine.MoveNext())
                        yield break;
                }
                catch (Exception e)
                {
                    string message = "Coroutine Exception: " + e.Message;

                    CoroutineException error = new CoroutineException(message, e);

                    Debug.LogException(error);

                    yield break;
                }

                yield return coroutine.Current;
            }
	    }
	}
}
#endif
