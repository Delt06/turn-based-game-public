using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Features._Shared.Mediation
{
    [RequireComponent(typeof(ICharacterMediator))]
    public class CharacterMediatorRefView : ComponentView<CharacterMediatorRef>
    {
        protected override void PostInitializeEntity(EcsPackedEntityWithWorld entity)
        {
            base.PostInitializeEntity(entity);
            entity.GetOrAdd<CharacterMediatorRef>().Mediator = GetComponent<ICharacterMediator>();
        }
    }
}