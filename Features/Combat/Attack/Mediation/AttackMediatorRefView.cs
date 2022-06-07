using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Features.Combat.Attack.Mediation
{
    [RequireComponent(typeof(IAttackMediator))]
    public class AttackMediatorRefView : ComponentView<AttackMediatorRef>
    {
        protected override void PostInitializeEntity(EcsPackedEntityWithWorld entity)
        {
            base.PostInitializeEntity(entity);
            entity.GetOrAdd<AttackMediatorRef>().Mediator = GetComponent<IAttackMediator>();
        }
    }
}