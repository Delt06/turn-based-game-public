using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Features._Shared;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Leveling.Stats._Base
{
    public class StatUpgradeRequestHandlingSystem<TStatData> : IEcsRunSystem where TStatData : struct, IStatData
    {
        private readonly EcsReadWritePool<StatsPointsData> _points;
        private readonly EcsFilter _requestsFilter;
        private readonly EcsFilter _statFilter;
        private readonly EcsReadWritePool<TStatData> _stats;
        private readonly EcsWorld _world;

        [UsedImplicitly]
        public StatUpgradeRequestHandlingSystem(EcsWorld world)
        {
            _world = world;
            _requestsFilter = world.Filter<StatUpgradeRequest<TStatData>>().End();
            _statFilter = world.Filter<StatsPointsData>().Inc<TStatData>().Inc<PlayerTag>().End();
            _points = world.GetPool<StatsPointsData>().AsReadWrite();
            _stats = world.GetPool<TStatData>().AsReadWrite();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var _ in _requestsFilter)
            {
                foreach (var i in _statFilter)
                {
                    ref readonly var statsPointsData = ref _points.Read(i);
                    if (statsPointsData.FreePoints <= 0) continue;

                    Upgrade(i);
                    break;
                }
            }
        }

        private void Upgrade(int i)
        {
            ref var statsPointsData = ref _points.Modify(i);
            statsPointsData.FreePoints--;
            ref var statData = ref _stats.Modify(i);
            statData.Level++;

            var entity = _world.PackEntityWithWorld(i);
            entity.OnUpdated<StatsPointsData>();
            entity.OnUpdated<TStatData>();
        }
    }
}