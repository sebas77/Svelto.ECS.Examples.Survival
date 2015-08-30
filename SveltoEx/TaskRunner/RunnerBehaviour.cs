#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_IPHONE || UNITY_ANDROID || UNITY_EDITOR
using UnityEngine;

public class RunnerBehaviour: MonoBehaviour
{
	static public bool isQuitting = false;
	
	void OnApplicationQuit()
    {
        isQuitting = true;
    }
}
#endif
