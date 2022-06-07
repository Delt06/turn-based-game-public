using System;
using Features._Shared.Data;
using UnityEngine;

namespace Features.Combat
{
    [CreateAssetMenu(menuName = AssetPath + "Combat")]
    public class CombatStaticData : StaticDataBase
    {
        [Min(0f)]
        public float AttackDuration = 1f;
        [Range(0f, 1f)]
        public float NormalizedAttackDealDamageTime;

        public RegenerationData Regeneration;

        [Serializable]
        public struct RegenerationData
        {
            [Min(0f)]
            public float MinTimeSinceLastAttack;

            [Min(0f)]
            public float RelativeRate;
        }
    }
}