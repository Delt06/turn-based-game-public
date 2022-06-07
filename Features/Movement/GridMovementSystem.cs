using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Features._Shared.Busy;
using Features.Grid;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Movement
{
    public class GridMovementSystem : IEcsRunSystem
    {
        private readonly IGameBusyService _busyService;
        private readonly EcsFilter _filter;
        private readonly IGridService _grid;
        private readonly EcsPool<GridPositionChangedEvent> _positionChangedEvents;
        private readonly EcsPool<GridPosition> _positions;
        private readonly EcsReadOnlyPool<MovementRequest> _requests;

        [UsedImplicitly]
        public GridMovementSystem(IGameBusyService busyService, IGridService grid, EcsWorld world)
        {
            _busyService = busyService;
            _grid = grid;
            _filter = world.Filter<GridPosition>().Inc<MovementRequest>().End();
            _positions = world.GetPool<GridPosition>();
            _requests = world.GetPool<MovementRequest>().AsReadOnly();
            _positionChangedEvents = world.GetPool<GridPositionChangedEvent>();
        }

        public void Run(EcsSystems systems)
        {
            if (_busyService.IsBusy) return;

            foreach (var i in _filter)
            {
                ref var gridPosition = ref _positions.Get(i);
                ref readonly var request = ref _requests.Read(i);
                var to = request.To;
                if (!to.IsNeighborTo(gridPosition)) continue;
                if (_grid.TryGetEntityAtPosition(to, out _)) continue;

                ref var gridPositionChangedEvent = ref _positionChangedEvents.GetOrAdd(i);
                gridPositionChangedEvent.From = gridPosition;

                gridPosition = to;
                gridPositionChangedEvent.To = gridPosition;
                _busyService.MakeBusy();
                break;
            }
        }
    }
}