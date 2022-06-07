using DELTation.LeoEcsExtensions.Utilities;
using Features._Shared.Ui;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Stamina
{
    public class PlayerStaminaViewSystem : IEcsRunSystem
    {
        private readonly EcsFilter _filter;
        private readonly EcsReadOnlyPool<StaminaData> _stamina;
        private readonly PlayerStaminaView _view;

        [UsedImplicitly]
        public PlayerStaminaViewSystem(UiRoot uiRoot, EcsWorld world)
        {
            _view = uiRoot.PlayerStaminaView;
            _filter = world.FilterAndIncUpdateOf<StaminaData>().End();
            _stamina = world.GetPool<StaminaData>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var staminaData = ref _stamina.Read(i);
                var fillRatio = staminaData.Stamina / staminaData.MaxStamina;
                _view.Display(fillRatio);
            }
        }
    }
}