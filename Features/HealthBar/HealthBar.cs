using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Features.HealthBar
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] [Required] private Image _fill = default!;
        [SerializeField] [Required] private Canvas _canvas = default!;

        public void Display(float healthValue, float healthMaxValue)
        {
            _fill.fillAmount = healthValue / healthMaxValue;
        }

        public void Hide() => _canvas.enabled = false;
    }
}