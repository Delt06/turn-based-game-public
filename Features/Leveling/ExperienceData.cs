using System;
using UnityEngine;

namespace Features.Leveling
{
    [Serializable]
    public struct ExperienceData
    {
        [Min(0)]
        public float Experience;
        [Min(0)]
        public int Level;
    }
}