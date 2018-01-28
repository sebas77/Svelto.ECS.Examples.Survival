using Svelto.ECS.Example.Survive.Components.HUD;
using UnityEngine;
using UnityEngine.UI;

namespace Svelto.ECS.Example.Survive.Implementors.HUD
{
    public class HealthSliderImplementor : MonoBehaviour, IHealthSliderComponent, IImplementor
    {
        public Slider healthSlider { get; private set; }

        void Awake()
        {
            healthSlider = GetComponent<Slider>();
        }
    }
}
