using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Features.Combat.Damage;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Combat.Attack
{
    [UsedImplicitly]
    public class SecondaryAttackSystem : IEcsRunSystem
    {
        private readonly EcsPool<AttackInProgress> _attacks;
        private readonly EcsPool<AttackStartedEvent> _attackStartedEvents;
        private readonly EcsReadOnlyPool<DamageData> _damages;
        private readonly EcsFilter _filter;
        private readonly EcsWorld _world;

        public SecondaryAttackSystem(EcsWorld world)
        {
            _world = world;
            _filter = world.Filter<AttackInProgress>().Inc<AttackStartedEvent>().End();
            _attacks = world.GetPool<AttackInProgress>();
            _damages = world.GetPool<DamageData>().AsReadOnly();
            _attackStartedEvents = world.GetPool<AttackStartedEvent>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var attackInProgress = ref _attacks.Get(i);
                if (!attackInProgress.Primary) continue;

                var attackedEntity = attackInProgress.AttackedEntity;
                if (!attackedEntity.Unpack(out _, out var attackedEntityIdx)) continue;
                if (!_damages.Has(attackedEntityIdx)) continue;
                if (_attacks.Has(attackedEntityIdx)) continue;

                ref var otherAttackInProgress = ref _attacks.Add(attackedEntityIdx);
                otherAttackInProgress.AttackedEntity = _world.PackEntityWithWorld(i);
                otherAttackInProgress.Primary = false;
                _attackStartedEvents.GetOrAdd(attackedEntityIdx);
            }
        }
    }
}