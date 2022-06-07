using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Leveling.Stats
{
    public class GiveStatPointsForLevelUpSystem : IEcsRunSystem
    {
        private readonly LevelingStaticData _data;
        private readonly EcsPool<LevelUpEvent> _events;
        private readonly EcsFilter _filter;
        private readonly EcsReadWritePool<StatsPointsData> _statPoints;

        [UsedImplicitly]
        public GiveStatPointsForLevelUpSystem(LevelingStaticData data, EcsWorld world)
        {
            _data = data;
            _filter = world.Filter<LevelUpEvent>().End();
            _events = world.GetPool<LevelUpEvent>();
            _statPoints = world.GetPool<StatsPointsData>().AsReadWrite();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                var entity = _events.Get(i).Entity;
                if (!entity.Unpack(out _, out var entityIdx)) continue;
                if (!_statPoints.Has(entityIdx)) continue;

                _statPoints.Modify(entityIdx).FreePoints += _data.StatPointsPerLevel;
            }
        }
    }
}