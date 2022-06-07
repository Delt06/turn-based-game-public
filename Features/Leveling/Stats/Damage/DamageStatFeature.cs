using Features._Shared.Ui;
using Features.Combat.Damage;
using Features.Leveling.Stats._Base;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Leveling.Stats.Damage
{
    public class DamageStatFeature : StatFeature<DamageData, DamageStatData>
    {
        [UsedImplicitly]
        public DamageStatFeature(LevelingStaticData levelingStaticData, UiRoot uiRoot, EcsWorld world) : base(
            levelingStaticData, uiRoot, world
        ) { }

        protected override void ApplyStatData(LevelingStaticData levelingStaticData, ref DamageData data,
            in DamageStatData statData)
        {
            data.Value = levelingStaticData.GetDamage(statData.Level);
        }

        protected override PlayerStatView SelectView(UiRoot uiRoot) => uiRoot.PlayerDamageStatView;
    }
}