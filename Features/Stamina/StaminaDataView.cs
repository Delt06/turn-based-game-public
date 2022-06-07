using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views.Components;
using Leopotam.EcsLite;

namespace Features.Stamina
{
    public class StaminaDataView : ComponentView<StaminaData>
    {
        protected override void PostInitializeEntity(EcsPackedEntityWithWorld entity)
        {
            base.PostInitializeEntity(entity);
            ref var staminaData = ref entity.GetOrAdd<StaminaData>();
            staminaData.Stamina = staminaData.MaxStamina;
            entity.OnUpdated<StaminaData>();
        }
    }
}