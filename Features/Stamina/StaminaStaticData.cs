using Features._Shared.Data;
using UnityEngine;

namespace Features.Stamina
{
    [CreateAssetMenu(menuName = AssetPath + "Stamina")]
    public class StaminaStaticData : StaticDataBase
    {
        [SerializeField] [Min(0f)] private float _staminaToAttack = 10f;
        [SerializeField] [Min(0f)] private float _relativeRegenerationRate = 1f;

        public float StaminaToAttack => _staminaToAttack;
        public float RelativeRegenerationRate => _relativeRegenerationRate;
    }
}