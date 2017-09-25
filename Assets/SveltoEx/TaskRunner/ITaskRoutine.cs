using System;
using System.Collections;

namespace Svelto.Tasks
{
    public interface ITaskRoutine
    {
        ITaskRoutine SetEnumeratorProvider(Func<IEnumerator> taskGenerator);
        ITaskRoutine SetEnumerator(IEnumerator taskGenerator);
        ITaskRoutine SetScheduler(IRunner runner);

        EnumeratorWrapper Start(Action<PausableTaskException> onFail = null, Action onStop = null);
        EnumeratorWrapper ThreadSafeStart(Action<PausableTaskException> onFail = null, Action onStop = null);
     
        void Pause();
        void Resume();
        void Stop();
    }
}
