using GUIComponents;
using UnityEngine;
using UnityEngine.UI;

namespace CompleteProject
{
    public class HealthSlider : MonoBehaviour, IHealthSliderComponent
    {
        Slider IHealthSliderComponent.healthSlider { get { return _slider; } }

        void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        Slider _slider;
    }
}
