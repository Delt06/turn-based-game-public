using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

namespace Features.Stamina
{
    public class StaminaRegenerationSystem : IEcsRunSystem
    {
        private readonly StaminaStaticData _data;
        private readonly EcsFilter _filter;
        private readonly EcsReadWritePool<StaminaData> _stamina;

        [UsedImplicitly]
        public StaminaRegenerationSystem(StaminaStaticData data, EcsWorld world)
        {
            _data = data;
            _filter = world.Filter<StaminaData>().End();
            _stamina = world.GetPool<StaminaData>().AsReadWrite();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var staminaDataRo = ref _stamina.Read(i);
                var newStamina = Mathf.MoveTowards(staminaDataRo.Stamina, staminaDataRo.MaxStamina,
                    _data.RelativeRegenerationRate * staminaDataRo.MaxStamina * Time.deltaTime
                );
                if (Mathf.Approximately(staminaDataRo.Stamina, newStamina)) continue;

                UpdateStamina(i, newStamina);
            }
        }

        private void UpdateStamina(int i, float newStamina)
        {
            ref var staminaData = ref _stamina.Modify(i);
            staminaData.Stamina = newStamina;
        }
    }
}