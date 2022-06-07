using DELTation.LeoEcsExtensions.Utilities;
using Features.Death;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.HealthBar
{
    [UsedImplicitly]
    public class HealthBarHideOnDeathSystem : IEcsRunSystem
    {
        private readonly EcsReadOnlyPool<HealthBarData> _bars;
        private readonly EcsFilter _filter;

        public HealthBarHideOnDeathSystem(EcsWorld world)
        {
            _filter = world.Filter<HealthBarData>().Inc<GridEntityDestructionRequest>().End();
            _bars = world.GetPool<HealthBarData>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                _bars.Read(i).HealthBar.Hide();
            }
        }
    }
}