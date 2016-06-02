using System;
using System.Collections;

namespace Svelto.Tasks.Internal
{
	internal class PausableTask: IEnumerator
	{
		public object Current
        {
            get
            {
                if (_enumerator != null)
                    return _enumerator.Current;

                return null;
            }
        }

		public PausableTask(IEnumerator enumerator, IRunner runner)
		{
            if (enumerator is SingleTask || enumerator is PausableTask || enumerator is AsyncTask)
				throw new ArgumentException("Internal task used outside the framework scope");

            _enumerator = enumerator;

			_runner = runner;
		}

        public PausableTask(IRunner runner)
		{
            _runner = runner;
		}

        public PausableTask(TaskCollection collection, IRunner runner)
		{
            _enumerator = collection.GetEnumerator(); //SingleTask is needed to be able to pause sub tasks

			_runner = runner;
		}

        public bool MoveNext()
		{
			if (_stopped || _runner.stopped)
				return false;

			if (_runner.paused == false && _paused == false)
				return _enumerator.MoveNext();

			return true;
		}

		public void Reset()
		{}

		public void Start(bool isSimple)
		{
            SetTask(_enumerator, isSimple);

            _runner.StartCoroutine(this);
		}

        public void Start(IEnumerator task, bool isSimple)
		{
            SetTask(task, isSimple);

            _runner.StartCoroutine(this);
		}

		public void Pause()
		{
			_paused = true;
		}

		public void Resume()
		{
			_paused = false;
		}

		public void Stop()
		{
			_stopped = true;
			_enumerator = null;
		}

        void SetTask(IEnumerator task, bool isSimple)
        {
            if (isSimple == false)
            {
                if (_enumerator is SingleTask)
                    (_enumerator as SingleTask).Reuse(task);
                else
                    _enumerator = new SingleTask(task);
            }
            else
                _enumerator = task;
        }

		IRunner 		_runner;
		bool			_stopped = false;
		IEnumerator 	_enumerator;
		bool			_paused = false;
	}
}

