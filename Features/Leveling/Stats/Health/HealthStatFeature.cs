using Features._Shared.Ui;
using Features.Combat.Health;
using Features.Leveling.Stats._Base;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Leveling.Stats.Health
{
    public class HealthStatFeature : StatFeature<HealthData, HealthStatData>
    {
        [UsedImplicitly]
        public HealthStatFeature(LevelingStaticData levelingStaticData, UiRoot uiRoot, EcsWorld world) : base(
            levelingStaticData, uiRoot, world
        ) { }

        protected override void ApplyStatData(LevelingStaticData levelingStaticData, ref HealthData data,
            in HealthStatData statData)
        {
            var currentRatio = data.Value / data.MaxValue;
            var maxValue = levelingStaticData.GetMaxHealth(statData.Level);
            data.MaxValue = maxValue;
            data.Value = currentRatio * maxValue;
        }

        protected override PlayerStatView SelectView(UiRoot uiRoot) => uiRoot.PlayerHealthStatView;
    }
}