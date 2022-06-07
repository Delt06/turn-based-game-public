using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Leveling
{
    public class PlayerExperienceView : MonoBehaviour
    {
        [SerializeField] [Required] private Image _experienceFill = default!;
        [SerializeField] [Required] private TMP_Text _levelText = default!;
        [SerializeField] [TextArea] private string _levelTextFormat = "Lv\n{0:0}";

        public void Display(float experienceFill, int displayedLevel)
        {
            _experienceFill.fillAmount = experienceFill;
            _levelText.SetText(_levelTextFormat, displayedLevel);
        }
    }
}