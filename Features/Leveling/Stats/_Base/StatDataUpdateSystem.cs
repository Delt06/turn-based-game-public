using DELTation.LeoEcsExtensions.Utilities;
using Leopotam.EcsLite;

namespace Features.Leveling.Stats._Base
{
    public class StatDataUpdateSystem<TData, TStatData> : IEcsRunSystem where TStatData : struct where TData : struct
    {
        public delegate void ApplyStatDataHandler(LevelingStaticData levelingStaticData, ref TData data,
            in TStatData statData);

        private readonly ApplyStatDataHandler _applyStatDataHandler;
        private readonly EcsReadWritePool<TData> _data;
        private readonly EcsFilter _filter;
        private readonly LevelingStaticData _levelingStaticData;
        private readonly EcsReadWritePool<TStatData> _statData;

        public StatDataUpdateSystem(LevelingStaticData levelingStaticData, ApplyStatDataHandler applyStatDataHandler,
            EcsWorld world)
        {
            _levelingStaticData = levelingStaticData;
            _applyStatDataHandler = applyStatDataHandler;
            _filter = world.Filter<TData>().IncComponentAndUpdateOf<TStatData>().End();
            _data = world.GetPool<TData>().AsReadWrite();
            _statData = world.GetPool<TStatData>().AsReadWrite();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var data = ref _data.Modify(i);
                ref var statData = ref _statData.Modify(i);
                _applyStatDataHandler(_levelingStaticData, ref data, statData);
            }
        }
    }
}