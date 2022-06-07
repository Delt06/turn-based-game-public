using Features._Shared.Ui;
using Features.Leveling.Stats._Base;
using Features.Stamina;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Leveling.Stats.Stamina
{
    public class StaminaStatFeature : StatFeature<StaminaData, StaminaStatData>
    {
        [UsedImplicitly]
        public StaminaStatFeature(LevelingStaticData levelingStaticData, UiRoot uiRoot, EcsWorld world) : base(
            levelingStaticData,
            uiRoot, world
        ) { }

        protected override void ApplyStatData(LevelingStaticData levelingStaticData, ref StaminaData data,
            in StaminaStatData statData)
        {
            var ratio = data.Stamina / data.MaxStamina;
            data.MaxStamina = levelingStaticData.GetMaxStamina(statData.Level);
            data.Stamina = ratio * data.MaxStamina;
        }

        protected override PlayerStatView SelectView(UiRoot uiRoot) => uiRoot.PlayerStaminaStatView;
    }
}