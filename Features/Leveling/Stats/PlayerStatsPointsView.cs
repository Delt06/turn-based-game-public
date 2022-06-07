using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Leveling.Stats
{
    public class PlayerStatsPointsView : MonoBehaviour
    {
        [SerializeField] [Required] private TMP_Text _text = default!;
        [SerializeField] [Required] private GameObject _content = default!;

        public void Display(int points)
        {
            if (points > 0)
            {
                _text.SetText("{0:0}", points);
                _content.SetActive(true);
            }
            else
            {
                _content.SetActive(false);
            }
        }
    }
}