using DELTation.LeoEcsExtensions.Utilities;
using Features.Combat.Attack;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Stamina
{
    public class StaminaAttackRequirementSystem : IEcsRunSystem
    {
        private readonly StaminaStaticData _data;
        private readonly EcsFilter _filter;
        private readonly EcsPool<AttackRequest> _requests;
        private readonly EcsReadOnlyPool<StaminaData> _stamina;

        [UsedImplicitly]
        public StaminaAttackRequirementSystem(StaminaStaticData data, EcsWorld world)
        {
            _data = data;
            _filter = world.Filter<StaminaData>().Inc<AttackRequest>().End();
            _stamina = world.GetPool<StaminaData>().AsReadOnly();
            _requests = world.GetPool<AttackRequest>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var staminaData = ref _stamina.Read(i);
                if (staminaData.Stamina >= _data.StaminaToAttack) continue;

                _requests.Del(i);
            }
        }
    }
}