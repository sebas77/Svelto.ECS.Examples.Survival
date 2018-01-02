#if UNITY_5 || UNITY_5_3_OR_NEWER
using Svelto.DataStructures;
using Svelto.Tasks.Internal;
using UnityEngine;

namespace Svelto.Tasks
{
    public class CoroutineMonoRunner : MonoRunner
    {
        public CoroutineMonoRunner(string name)
        {
            _go = UnityCoroutineRunner.InitializeGameobject(name);
            var coroutines = new FasterList<IPausableTask>(NUMBER_OF_INITIAL_COROUTINE);

            RunnerBehaviour runnerBehaviour = _go.AddComponent<RunnerBehaviour>();
            var runnerBehaviourForUnityCoroutine = _go.AddComponent<RunnerBehaviour>();

            _info = new UnityCoroutineRunner.RunningTasksInfo() { runnerName = name };

            runnerBehaviour.StartCoroutine(UnityCoroutineRunner.Process
                (_newTaskRoutines, coroutines, _flushingOperation, _info,
                 UnityCoroutineRunner.StandardTasksFlushing,
                 runnerBehaviourForUnityCoroutine, StartCoroutine));
        }

        public override void DisposeRunner()
        {
            GameObject.Destroy(_go);
        }

        protected override UnityCoroutineRunner.RunningTasksInfo info
        { get { return _info; } }

        protected override ThreadSafeQueue<IPausableTask> newTaskRoutines
        { get { return _newTaskRoutines; } }

        protected override UnityCoroutineRunner.FlushingOperation flushingOperation
        { get { return _flushingOperation; } }

        readonly ThreadSafeQueue<IPausableTask>         _newTaskRoutines = new ThreadSafeQueue<IPausableTask>();
        readonly UnityCoroutineRunner.FlushingOperation _flushingOperation = new UnityCoroutineRunner.FlushingOperation();
        readonly UnityCoroutineRunner.RunningTasksInfo  _info;
        GameObject _go;

        const int NUMBER_OF_INITIAL_COROUTINE = 3;
    }
}
#endif