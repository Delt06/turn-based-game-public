using DELTation.LeoEcsExtensions.Utilities;
using Features.Combat.Attack;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine.Assertions;

namespace Features.Stamina
{
    public class StaminaAttackConsumptionSystem : IEcsRunSystem
    {
        private readonly StaminaStaticData _data;
        private readonly EcsFilter _filter;
        private readonly EcsReadWritePool<StaminaData> _stamina;

        [UsedImplicitly]
        public StaminaAttackConsumptionSystem(StaminaStaticData data, EcsWorld world)
        {
            _data = data;
            _filter = world.Filter<StaminaData>().Inc<AttackStartedEvent>().End();
            _stamina = world.GetPool<StaminaData>().AsReadWrite();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref var staminaData = ref _stamina.Modify(i);
                var consumedStamina = _data.StaminaToAttack;
                Assert.IsTrue(staminaData.Stamina >= consumedStamina);

                staminaData.Stamina -= consumedStamina;
            }
        }
    }
}