using System.Collections;

namespace Svelto.Tasks
{
	public class TimingOutTask: IEnumerator
	{
		public object Current 		{ get { return _enumerator.Current; } }

		public TimingOutTask(float milliseconds, System.Action action):this(milliseconds, action, false)
		{}

		public TimingOutTask(float milliseconds, System.Action action, bool repeat)
		{
			if (repeat == true)
				action();

			_enumerator = Timeout(action, milliseconds, repeat);
		}

		IEnumerator Timeout(System.Action action, float milliseconds, bool repeat)
		{
            System.DateTime endTime = System.DateTime.UtcNow.AddMilliseconds(milliseconds);

			while (System.DateTime.UtcNow < endTime)
				yield return null;

			action();

			if (repeat == true)
				yield return Timeout(action, milliseconds, true);
		}

		virtual public bool MoveNext()
		{
			return _enumerator.MoveNext();
		}
		public void Reset()
		{
			_enumerator.Reset();
		}

		private IEnumerator 		_enumerator;
	}
}

