using Components.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Implementators.HUD
{
    public class ScoreManager : MonoBehaviour, IScoreComponent
    {
        int IScoreComponent.score { get { return _score; } set { _score = value; _text.text = "score: " + _score; } }

        void Awake ()
        {
            // Set up the reference.
            _text = GetComponent <Text> ();

            // Reset the score.
            _score = 0;
        }

        int     _score;
        Text    _text;
    }
}
