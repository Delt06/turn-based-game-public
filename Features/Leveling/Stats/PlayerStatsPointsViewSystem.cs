using DELTation.LeoEcsExtensions.Utilities;
using Features._Shared;
using Features._Shared.Ui;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Leveling.Stats
{
    public class PlayerStatsPointsViewSystem : IEcsRunSystem
    {
        private readonly EcsFilter _filter;
        private readonly EcsReadOnlyPool<StatsPointsData> _points;
        private readonly PlayerStatsPointsView _view;

        [UsedImplicitly]
        public PlayerStatsPointsViewSystem(UiRoot uiRoot, EcsWorld world)
        {
            _view = uiRoot.PlayerStatsPointsView;
            _filter = world.FilterAndIncUpdateOf<StatsPointsData>().Inc<PlayerTag>().End();
            _points = world.GetPool<StatsPointsData>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var pointsData = ref _points.Read(i);
                _view.Display(pointsData.FreePoints);
            }
        }
    }
}