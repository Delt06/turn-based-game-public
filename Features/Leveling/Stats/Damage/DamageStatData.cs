using System;
using Features.Leveling.Stats._Base;
using UnityEngine;

namespace Features.Leveling.Stats.Damage
{
    [Serializable]
    public struct DamageStatData : IStatData
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