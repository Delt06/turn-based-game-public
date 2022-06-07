using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Combat.Damage
{
    [UsedImplicitly]
    public class FixedDamageIntakeSystem : IEcsRunSystem
    {
        private readonly EcsFilter _filter;
        private readonly EcsReadOnlyPool<FixedDamageIntake> _intakes;
        private readonly EcsPool<TakeDamageRequest> _requests;

        public FixedDamageIntakeSystem(EcsWorld world)
        {
            _filter = world.Filter<TakeDamageRequest>().End();
            _requests = world.GetPool<TakeDamageRequest>();
            _intakes = world.GetPool<FixedDamageIntake>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var takeDamageRequest = ref _requests.Get(i);
                var attackedEntity = takeDamageRequest.AttackedEntity;
                if (attackedEntity.Unpack(out _, out var attackedEntityIdx) && _intakes.Has(attackedEntityIdx))
                    takeDamageRequest.Damage = _intakes.Read(attackedEntityIdx).Damage;
            }
        }
    }
}