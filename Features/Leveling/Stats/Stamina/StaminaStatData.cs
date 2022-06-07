using System;
using Features.Leveling.Stats._Base;
using UnityEngine;

namespace Features.Leveling.Stats.Stamina
{
    [Serializable]
    public struct StaminaStatData : IStatData
    {
        [Min(0)]
        public int Level;

        int IStatData.Level
        {
            get => Level;
            set => Level = value;
        }
    }
}