using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views.Components;
using Leopotam.EcsLite;

namespace Features.Leveling.Stats._Base
{
    public class StatComponentView<TStatData> : ComponentView<TStatData> where TStatData : struct, IStatData
    {
        protected override void PostInitializeEntity(EcsPackedEntityWithWorld entity)
        {
            base.PostInitializeEntity(entity);
            entity.OnUpdated<TStatData>();
        }
    }
}