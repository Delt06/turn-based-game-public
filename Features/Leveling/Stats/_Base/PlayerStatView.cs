using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Leveling.Stats._Base
{
    public sealed class PlayerStatView : MonoBehaviour
    {
        [SerializeField] private string _format = "S{0:0}";
        [SerializeField] [Required] private TMP_Text _text = default!;
        [SerializeField] [Required] private Button _button = default!;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClicked);
        }

        private void OnValidate()
        {
            if (_text) Display(1);
        }

        private void OnClicked() => Clicked?.Invoke();

        public void Display(int level)
        {
            _text.SetText(_format, level);
        }

        public event Action Clicked;
    }
}