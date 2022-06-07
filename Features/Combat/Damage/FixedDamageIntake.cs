using System;
using UnityEngine;

namespace Features.Combat.Damage
{
    [Serializable]
    public struct FixedDamageIntake
    {
        [Min(0f)]
        public float Damage;
    }
}