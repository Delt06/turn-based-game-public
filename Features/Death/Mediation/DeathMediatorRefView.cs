using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Features.Death.Mediation
{
    [RequireComponent(typeof(IDeathMediator))]
    public class DeathMediatorRefView : ComponentView<DeathMediatorRef>
    {
        protected override void PostInitializeEntity(EcsPackedEntityWithWorld entity)
        {
            base.PostInitializeEntity(entity);
            entity.GetOrAdd<DeathMediatorRef>().Mediator = GetComponent<IDeathMediator>();
        }
    }
}