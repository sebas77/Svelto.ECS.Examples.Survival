using System.Diagnostics;
using UnityEngine;

public class PrintTime : MonoBehaviour {
    UnityEngine.UI.Text text;
    float frames;

    // Use this for initialization
    void Start () {
        text = GetComponent<UnityEngine.UI.Text>();
#if UNITY_EDITOR
        text.text = "this value has a different meaning in the Editor, look at the stats or profiler window instead";
        text.alignment = TextAnchor.MiddleLeft; text.horizontalOverflow = HorizontalWrapMode.Overflow;
     
#endif 
    }

#if !UNITY_EDITOR
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime > 1)
        {
            text.text = (Time.deltaTime * 1000).ToString("N6");
            _currentTime = 0;
        }
    }
#endif

    float _currentTime;
}
