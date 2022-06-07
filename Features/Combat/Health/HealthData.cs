using System;
using UnityEngine;

namespace Features.Combat.Health
{
    [Serializable]
    public struct HealthData
    {
        public float Value;
        [Min(0f)]
        public float MaxValue;
    }
}