using DELTation.LeoEcsExtensions.Utilities;
using Features.Combat.Attack.Mediation;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Combat.Attack
{
    public class AttackMediatorStartSystem : IEcsRunSystem
    {
        private readonly EcsReadOnlyPool<AttackInProgress> _attacksInProgress;
        private readonly CombatStaticData _combatData;
        private readonly EcsFilter _filter;
        private readonly EcsReadOnlyPool<AttackMediatorRef> _mediators;
        private readonly EcsWorld _world;

        [UsedImplicitly]
        public AttackMediatorStartSystem(CombatStaticData combatData, EcsWorld world)
        {
            _world = world;
            _combatData = combatData;
            _filter = world.Filter<AttackInProgress>().Inc<AttackMediatorRef>().Inc<AttackStartedEvent>().End();
            _attacksInProgress = world.GetPool<AttackInProgress>().AsReadOnly();
            _mediators = world.GetPool<AttackMediatorRef>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var attackInProgress = ref _attacksInProgress.Read(i);
                if (!attackInProgress.AttackedEntity.Unpack(out _, out var attackedEntityIdx)) continue;

                var mediator = _mediators.Read(i).Mediator;
                mediator.StartAttack(_world.PackEntityWithWorld(attackedEntityIdx), _combatData.AttackDuration,
                    _combatData.NormalizedAttackDealDamageTime
                );
            }
        }
    }
}