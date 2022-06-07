using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Stamina
{
    public class PlayerStaminaView : MonoBehaviour
    {
        [SerializeField] [Required] private Image _fill = default!;

        public void Display(float fillRatio)
        {
            _fill.fillAmount = fillRatio;
        }
    }
}