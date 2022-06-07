using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views.Components;
using Leopotam.EcsLite;

namespace Features.Leveling
{
    public class ExperienceDataView : ComponentView<ExperienceData>
    {
        protected override void PostInitializeEntity(EcsPackedEntityWithWorld entity)
        {
            base.PostInitializeEntity(entity);
            entity.OnUpdated<ExperienceData>();
        }
    }
}