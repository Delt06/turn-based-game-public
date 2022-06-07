using DELTation.LeoEcsExtensions.Utilities;
using Features._Shared.Busy;
using Features._Shared.Mediation;
using Features.Grid;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Combat.Attack
{
    public class GridAttackSystem : IEcsRunSystem
    {
        private readonly EcsReadOnlyPool<AttackRequest> _attackCommands;
        private readonly EcsPool<AttackInProgress> _attackInProgress;
        private readonly EcsPool<AttackStartedEvent> _attackStartedEvents;
        private readonly IGameBusyService _busyService;
        private readonly EcsFilter _filter;
        private readonly EcsReadOnlyPool<GridPosition> _gridPositions;
        private readonly IGridService _gridService;
        private readonly EcsWorld _world;

        [UsedImplicitly]
        public GridAttackSystem(IGameBusyService busyService, IGridService gridService, EcsWorld world)
        {
            _world = world;
            _busyService = busyService;
            _gridService = gridService;
            _filter = world.Filter<GridPosition>().Inc<AttackRequest>().Inc<CharacterMediatorRef>().End();
            _gridPositions = world.GetPool<GridPosition>().AsReadOnly();
            _attackCommands = world.GetPool<AttackRequest>().AsReadOnly();
            _attackInProgress = world.GetPool<AttackInProgress>();
            _attackStartedEvents = world.GetPool<AttackStartedEvent>();
        }

        public void Run(EcsSystems systems)
        {
            if (_busyService.IsBusy) return;

            foreach (var idx in _filter)
            {
                ref readonly var gridPosition = ref _gridPositions.Read(idx);
                ref readonly var gridAttackCommand = ref _attackCommands.Read(idx);
                if (!gridAttackCommand.TargetPosition.IsNeighborTo(gridPosition))
                    continue;

                if (!_gridService.TryGetEntityAtPosition(gridAttackCommand.TargetPosition, out var attackedEntity))
                    continue;
                if (!attackedEntity.Unpack(out _, out var attackedEntityIdx)) continue;
                if (attackedEntityIdx == idx) continue;

                ref var attackInProgress = ref _attackInProgress.Add(idx);
                attackInProgress.AttackedEntity = _world.PackEntityWithWorld(attackedEntityIdx);
                attackInProgress.Primary = true;
                _attackStartedEvents.Add(idx);
                _busyService.MakeBusy();
            }
        }
    }
}