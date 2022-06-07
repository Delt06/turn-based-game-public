using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views.Components;
using Leopotam.EcsLite;

namespace Features.Leveling.Stats
{
    public class StatsPointsDataView : ComponentView<StatsPointsData>
    {
        protected override void PostInitializeEntity(EcsPackedEntityWithWorld entity)
        {
            base.PostInitializeEntity(entity);
            entity.OnUpdated<StatsPointsData>();
        }
    }
}