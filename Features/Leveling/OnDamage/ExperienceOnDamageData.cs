using System;
using UnityEngine;

namespace Features.Leveling.OnDamage
{
    [Serializable]
    public struct ExperienceOnDamageData
    {
        [Min(0f)]
        public float Experience;
    }
}