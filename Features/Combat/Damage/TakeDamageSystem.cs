using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Features._Shared.TimeManagement;
using Features.Combat.Health;
using Features.Death;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Combat.Damage
{
    public class TakeDamageSystem : IEcsRunSystem
    {
        private readonly EcsPool<GridEntityDestructionRequest> _destructionRequests;
        private readonly EcsFilter _filter;
        private readonly EcsReadWritePool<HealthData> _health;
        private readonly EcsPool<KillerData> _killerData;
        private readonly EcsPool<LastTakeDamageTime> _lastTakeDamageTimes;
        private readonly EcsReadOnlyPool<TakeDamageRequest> _requests;
        private readonly ITime _time;
        private readonly EcsWorld _world;

        [UsedImplicitly]
        public TakeDamageSystem(ITime time, EcsWorld world)
        {
            _world = world;
            _time = time;
            _filter = world.Filter<TakeDamageRequest>().End();
            _requests = world.GetPool<TakeDamageRequest>().AsReadOnly();
            _health = world.GetPool<HealthData>().AsReadWrite();
            _lastTakeDamageTimes = world.GetPool<LastTakeDamageTime>();
            _destructionRequests = world.GetPool<GridEntityDestructionRequest>();
            _killerData = world.GetPool<KillerData>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var request = ref _requests.Read(i);
                var attackedEntity = request.AttackedEntity;
                if (!attackedEntity.Unpack(out _, out var attackedEntityIdx)) continue;
                if (!_health.Has(attackedEntityIdx)) continue;

                ref readonly var readOnlyHealth = ref _health.Read(attackedEntityIdx);
                if (readOnlyHealth.IsDead()) continue;

                DealDamage(attackedEntityIdx, request);
            }
        }

        private void DealDamage(int attackedEntityIdx, in TakeDamageRequest request)
        {
            ref var health = ref _health.Modify(attackedEntityIdx);

            health.Value -= request.Damage;
            _world.NewPackedEntityWithWorld().Add<TookDamageEvent>().Request = request;
            _lastTakeDamageTimes.GetOrAdd(attackedEntityIdx).Time = _time.Time;
            if (!health.IsDead()) return;

            _destructionRequests.GetOrAdd(attackedEntityIdx);
            if (request.Attacker.IsAlive())
                _killerData.Add(attackedEntityIdx).Killer = request.Attacker;
        }
    }
}