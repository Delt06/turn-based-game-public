using System;
using UnityEngine;

namespace Features.Combat.Damage
{
    [Serializable]
    public struct DamageData
    {
        [Min(0f)]
        public float Value;
    }
}