using DELTation.LeoEcsExtensions.Utilities;
using Features.Combat.Health;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.HealthBar
{
    public class HealthBarDisplaySystem : IEcsRunSystem
    {
        private readonly EcsReadOnlyPool<HealthBarData> _bars;
        private readonly EcsFilter _filter;
        private readonly EcsReadOnlyPool<HealthData> _health;

        [UsedImplicitly]
        public HealthBarDisplaySystem(EcsWorld world)
        {
            _filter = world.FilterAndIncUpdateOf<HealthData>().Inc<HealthBarData>().End();
            _health = world.GetPool<HealthData>().AsReadOnly();
            _bars = world.GetPool<HealthBarData>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var health = ref _health.Read(i);
                ref readonly var healthBarData = ref _bars.Read(i);
                healthBarData.HealthBar.Display(health.Value, health.MaxValue);
            }
        }
    }
}