using System;
using NaughtyAttributes;

namespace Features.HealthBar
{
    [Serializable]
    public struct HealthBarData
    {
        [Required]
        public HealthBar HealthBar;
    }
}