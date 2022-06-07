using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Composition;
using Features._Shared.Ui;
using Leopotam.EcsLite;

namespace Features.Leveling.Stats._Base
{
    public abstract class StatFeature<TData, TStatData> : PrebuiltFeature
        where TStatData : struct, IStatData where TData : struct
    {
        private readonly LevelingStaticData _levelingStaticData;
        private readonly UiRoot _uiRoot;
        private readonly EcsWorld _world;

        protected StatFeature(LevelingStaticData levelingStaticData, UiRoot uiRoot, EcsWorld world)
        {
            _levelingStaticData = levelingStaticData;
            _uiRoot = uiRoot;
            _world = world;
        }

        protected override void ConfigureBuilder(EcsFeatureBuilder featureBuilder)
        {
            featureBuilder
                .Add(new StatUpgradeRequestHandlingSystem<TStatData>(_world))
                .OneFrame<StatUpgradeRequest<TStatData>>()
                .Add(new StatDataUpdateSystem<TData, TStatData>(_levelingStaticData, ApplyStatData, _world))
                .Add(new StatDataViewUpdateSystem<TStatData>(SelectView(_uiRoot), _world))
                .OneFrame<UpdateEvent<TStatData>>()
                ;
        }

        protected abstract void ApplyStatData(LevelingStaticData levelingStaticData, ref TData data,
            in TStatData statData);

        protected abstract PlayerStatView SelectView(UiRoot uiRoot);
    }
}