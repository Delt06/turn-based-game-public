using DELTation.LeoEcsExtensions.Utilities;
using Features._Shared.TimeManagement;
using Features.Combat.Damage;
using Features.Combat.Health;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

namespace Features.Combat.Regeneration
{
    public class HealthRegenerationSystem : IEcsRunSystem
    {
        private readonly CombatStaticData _data;
        private readonly EcsReadWritePool<HealthData> _health;
        private readonly EcsReadOnlyPool<LastTakeDamageTime> _lastTakeDamageTimes;
        private readonly EcsFilter _noDamageFilter;
        private readonly EcsFilter _takeDamageFilter;
        private readonly ITime _time;

        [UsedImplicitly]
        public HealthRegenerationSystem(ITime time, CombatStaticData data, EcsWorld world)
        {
            _time = time;
            _data = data;
            _noDamageFilter = world.Filter<HealthData>().Exc<LastTakeDamageTime>().End();
            _takeDamageFilter = world.Filter<HealthData>().Inc<LastTakeDamageTime>().End();
            _health = world.GetPool<HealthData>().AsReadWrite();
            _lastTakeDamageTimes = world.GetPool<LastTakeDamageTime>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            var deltaTime = _time.DeltaTime;

            foreach (var i in _takeDamageFilter)
            {
                ref readonly var lastTakeDamageTime = ref _lastTakeDamageTimes.Read(i);
                if (_time.Time < lastTakeDamageTime.Time + _data.Regeneration.MinTimeSinceLastAttack) continue;

                Regenerate(i, deltaTime);
            }

            foreach (var i in _noDamageFilter)
            {
                Regenerate(i, deltaTime);
            }
        }

        private void Regenerate(int entity, float deltaTime)
        {
            ref readonly var readOnlyHealth = ref _health.Read(entity);
            var relativeRate = _data.Regeneration.RelativeRate;
            var rate = relativeRate * readOnlyHealth.MaxValue;
            var newHealth = Mathf.MoveTowards(readOnlyHealth.Value, readOnlyHealth.MaxValue,
                rate * deltaTime
            );
            if (Mathf.Approximately(readOnlyHealth.Value, newHealth)) return;

            _health.Modify(entity).Value = newHealth;
        }
    }
}