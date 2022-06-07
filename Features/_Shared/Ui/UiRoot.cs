using Features.Leveling;
using Features.Leveling.Stats;
using Features.Leveling.Stats._Base;
using Features.Stamina;
using NaughtyAttributes;
using UnityEngine;

namespace Features._Shared.Ui
{
    public class UiRoot : MonoBehaviour
    {
        [SerializeField] [Required] private PlayerExperienceView _playerExperienceView = default!;
        [SerializeField] [Required] private PlayerStatsPointsView _playerStatsPointsView = default!;
        [SerializeField] [Required] private PlayerStaminaView _playerStaminaView = default!;
        [SerializeField] [Required] private PlayerStatView _playerHealthStatView = default!;
        [SerializeField] [Required] private PlayerStatView _playerDamageStatView = default!;
        [SerializeField] [Required] private PlayerStatView _playerStaminaStatView = default!;

        public PlayerExperienceView PlayerExperienceView => _playerExperienceView;

        public PlayerStatsPointsView PlayerStatsPointsView => _playerStatsPointsView;

        public PlayerStaminaView PlayerStaminaView => _playerStaminaView;

        public PlayerStatView PlayerHealthStatView => _playerHealthStatView;

        public PlayerStatView PlayerDamageStatView => _playerDamageStatView;

        public PlayerStatView PlayerStaminaStatView => _playerStaminaStatView;
    }
}