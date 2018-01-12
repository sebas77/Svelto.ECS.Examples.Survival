#if UNITY_5 || UNITY_5_3_OR_NEWER
using Svelto.DataStructures;
using Svelto.Tasks.Internal;

namespace Svelto.Tasks
{
    public class LateMonoRunner : MonoRunner
    {
        public LateMonoRunner(string name)
            : base(name)
        {
            var coroutines = new FasterList<IPausableTask>(NUMBER_OF_INITIAL_COROUTINE);
            var runnerBehaviour = _gameObject.AddComponent<RunnerBehaviourLate>();
            var runnerBehaviourForUnityCoroutine = _gameObject.AddComponent<RunnerBehaviour>();

            _info = new UnityCoroutineRunner.RunningTasksInfo() { runnerName = name };

            runnerBehaviour.StartLateCoroutine(UnityCoroutineRunner.Process
                (_newTaskRoutines, coroutines, _flushingOperation, _info,
                 UnityCoroutineRunner.StandardTasksFlushing,
                 runnerBehaviourForUnityCoroutine, StartCoroutine));
        }

        protected override UnityCoroutineRunner.RunningTasksInfo info
        { get { return _info; } }

        protected override ThreadSafeQueue<IPausableTask> newTaskRoutines
        { get { return _newTaskRoutines; } }

        protected override UnityCoroutineRunner.FlushingOperation flushingOperation
        { get { return _flushingOperation; } }

        readonly ThreadSafeQueue<IPausableTask>          _newTaskRoutines = new ThreadSafeQueue<IPausableTask>();
        readonly UnityCoroutineRunner.FlushingOperation _flushingOperation = new UnityCoroutineRunner.FlushingOperation();
        readonly UnityCoroutineRunner.RunningTasksInfo  _info;

        const int NUMBER_OF_INITIAL_COROUTINE = 3;
    }
}
#endif