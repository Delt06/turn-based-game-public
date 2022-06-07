using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Features._Shared;
using Features.Combat.Attack;
using Features.Grid;
using Features.Movement;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.InputProcessing
{
    public class ClickHandlingSystem : IEcsRunSystem
    {
        private readonly EcsPool<AttackRequest> _attackRequests;
        private readonly EcsReadOnlyPool<CellClickEvent> _events;
        private readonly EcsFilter _eventsFilter;
        private readonly EcsFilter _gridPositionsFilter;
        private readonly IGridService _gridService;
        private readonly EcsPool<MovementRequest> _movementRequests;

        [UsedImplicitly]
        public ClickHandlingSystem(IGridService gridService, EcsWorld world)
        {
            _gridService = gridService;
            _eventsFilter = world.Filter<CellClickEvent>().End();
            _gridPositionsFilter = world.Filter<GridPosition>().Inc<PlayerTag>().End();
            _events = world.GetPool<CellClickEvent>().AsReadOnly();
            _attackRequests = world.GetPool<AttackRequest>();
            _movementRequests = world.GetPool<MovementRequest>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var iEvent in _eventsFilter)
            {
                ref readonly var clickPosition = ref _events.Read(iEvent).Position;

                foreach (var iGridPosition in _gridPositionsFilter)
                {
                    if (_gridService.TryGetEntityAtPosition(clickPosition, out _))
                        _attackRequests.GetOrAdd(iGridPosition).TargetPosition = clickPosition;
                    else
                        _movementRequests.GetOrAdd(iGridPosition).To = clickPosition;
                }

                break;
            }
        }
    }
}