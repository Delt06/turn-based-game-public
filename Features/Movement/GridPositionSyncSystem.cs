using DELTation.LeoEcsExtensions.Utilities;
using DG.Tweening;
using Features._Shared.Busy;
using Features._Shared.Mediation;
using Features.Grid;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Movement
{
    public class GridPositionSyncSystem : IEcsRunSystem
    {
        private readonly EcsFilter _filter;
        private readonly IGridService _grid;
        private readonly EcsReadOnlyPool<CharacterMediatorRef> _mediators;
        private readonly TweenCallback _onTweenComplete;
        private readonly EcsReadOnlyPool<GridPositionChangedEvent> _positionChangedEvents;
        private readonly EcsReadOnlyPool<MovementSpeed> _speeds;

        [UsedImplicitly]
        public GridPositionSyncSystem(IGridService grid, IGameBusyService busyService, EcsWorld world)
        {
            _grid = grid;
            _onTweenComplete = busyService.MakeFree;
            _filter = world.Filter<GridPositionChangedEvent>().Inc<CharacterMediatorRef>().Inc<MovementSpeed>().End();
            _positionChangedEvents = world.GetPool<GridPositionChangedEvent>().AsReadOnly();
            _mediators = world.GetPool<CharacterMediatorRef>().AsReadOnly();
            _speeds = world.GetPool<MovementSpeed>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var gridPositionChangedEvent = ref _positionChangedEvents.Read(i);
                var mediator = _mediators.Read(i).Mediator;
                var targetPosition = _grid.GridToWorldPosition(gridPositionChangedEvent.To);
                ref readonly var movementSpeed = ref _speeds.Read(i);
                mediator.Move(targetPosition, movementSpeed.Speed, movementSpeed.Ease, _onTweenComplete);
            }
        }
    }
}