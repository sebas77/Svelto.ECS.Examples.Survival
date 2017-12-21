using UnityEngine;

public class PrintTime : MonoBehaviour {
    UnityEngine.UI.Text text;
    
    // Use this for initialization
    void Start () {
        text = GetComponent<UnityEngine.UI.Text>();
#if UNITY_EDITOR
        text.text = "this value has a different meaning in the Editor, look at the stats or profiler window instead";
        text.alignment = TextAnchor.MiddleLeft; text.horizontalOverflow = HorizontalWrapMode.Overflow;
#else
        sw.Start();
#endif
    }

#if !UNITY_EDITOR
    void Update()
    {
        _currentTime = sw.Elapsed.Milliseconds;
        _frames++;
        if (_currentTime > 100)
        {
            text.text = (_currentTime / _frames).ToString("N6");
            _currentTime = 0; _frames = 0; sw.Reset(); sw.Start();
        }
    }

    int _currentTime; int _frames;
    Stopwatch sw = new Stopwatch();
#endif
}
