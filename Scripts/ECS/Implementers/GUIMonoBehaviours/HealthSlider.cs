using Components.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Implementators.HUD
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
