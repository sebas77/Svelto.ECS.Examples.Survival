using GUIComponents;
using UnityEngine;
using UnityEngine.UI;

namespace CompleteProject
{
    public class DamageHUD: MonoBehaviour, IDamageHUDComponent
    {
        public float flashSpeed = 5f;                               // The speed the damageImage will fade at.
        public Color flashColour = new Color(1f, 0f, 0f, 0.1f);     // The colour the damageImage is set to, to flash.

        Image IDamageHUDComponent.damageImage { get { return _image; } }
        float IDamageHUDComponent.flashSpeed { get { return flashSpeed; } }
        Color IDamageHUDComponent.flashColor { get { return flashColour; } }

        void Awake()
        {
            _image = GetComponent<Image>();
        }

        Image _image;
    }
}
