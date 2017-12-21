using UnityEngine;

public class PrintIteration : MonoBehaviour, IPrintStuffComponent
{
    public int iterations
    {
        get { return _iterations; }

        set { _iterations = value + 1; _text.text = _iterations.ToString();  }
    }

    // Use this for initialization
    void Start()
    {
        _text = GetComponent<UnityEngine.UI.Text>();

        _text.alignment = TextAnchor.MiddleLeft; _text.horizontalOverflow = HorizontalWrapMode.Overflow;
    }

    int _iterations;
    UnityEngine.UI.Text _text;
}
