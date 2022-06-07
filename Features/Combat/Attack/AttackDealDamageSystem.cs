using DELTation.LeoEcsExtensions.Utilities;
using Features.Combat.Damage;
using Features.Combat.Health;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Combat.Attack
{
    [UsedImplicitly]
    public class AttackDealDamageSystem : IEcsRunSystem
    {
        private readonly EcsReadOnlyPool<AttackInProgress> _attacksInProgress;
        private readonly EcsReadOnlyPool<DamageData> _damages;
        private readonly EcsFilter _filter;
        private readonly EcsReadOnlyPool<HealthData> _healths;
        private readonly EcsReadWritePool<TakeDamageRequest> _takeDamageRequests;
        private readonly EcsWorld _world;

        public AttackDealDamageSystem(EcsWorld world)
        {
            _world = world;
            _filter = world.Filter<AttackInProgress>()
                    .Inc<AttackDealDamageRequest>()
                    .Inc<DamageData>()
                    .End()
                ;
            _attacksInProgress = world.GetPool<AttackInProgress>().AsReadOnly();
            _takeDamageRequests = world.GetPool<TakeDamageRequest>().AsReadWrite();
            _damages = world.GetPool<DamageData>().AsReadOnly();
            _healths = world.GetPool<HealthData>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var attackInProgress = ref _attacksInProgress.Read(i);
                ref readonly var damage = ref _damages.Read(i);

                var attackedEntity = attackInProgress.AttackedEntity;
                if (!attackedEntity.Unpack(out _, out var attackedEntityIdx)) continue;
                if (!_healths.Has(attackedEntityIdx)) continue;

                RequestTakingDamage(damage, attackedEntity, _world.PackEntityWithWorld(i));
            }
        }

        private void RequestTakingDamage(in DamageData damageData, in EcsPackedEntityWithWorld attackedEntity,
            EcsPackedEntityWithWorld attacker)
        {
            ref var takeDamageRequest = ref _takeDamageRequests.Add(_world.NewEntity());
            takeDamageRequest.Damage = damageData.Value;
            takeDamageRequest.AttackedEntity = attackedEntity;
            takeDamageRequest.Attacker = attacker;
        }
    }
}