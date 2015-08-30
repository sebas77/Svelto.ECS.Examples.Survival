#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_IPHONE || UNITY_ANDROID || UNITY_EDITOR
using System.Collections;
using UnityEngine;

namespace Svelto.Tasks
{
	public class YieldInstructionTask: IEnumerator
	{
		public object Current 		{ get { return _enumerator.Current; } }
				 
		public YieldInstructionTask(YieldInstruction instruction)
		{
			_enumerator = ConvertIt(instruction);
		}
		
		IEnumerator ConvertIt(YieldInstruction instruction)
		{
			yield return instruction;
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
#endif
