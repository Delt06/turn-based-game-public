using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views.Components;
using Leopotam.EcsLite;

namespace Features.Combat.Health
{
    public class HealthView : ComponentView<HealthData>
    {
        protected override void PostInitializeEntity(EcsPackedEntityWithWorld entity)
        {
            base.PostInitializeEntity(entity);
            ref var health = ref entity.GetOrAdd<HealthData>();
            health.Value = health.MaxValue;
            entity.OnUpdated<HealthData>();
        }
    }
}