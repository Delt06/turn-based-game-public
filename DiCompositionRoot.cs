using DELTation.DIFramework;
using DELTation.DIFramework.Containers;
using Features._Shared.Busy;
using Features._Shared.TimeManagement;
using Features._Shared.Ui;
using Features.Combat;
using Features.Grid;
using Features.Leveling;
using Features.Stamina;
using Features.Vfx;
using NaughtyAttributes;
using UnityEngine;

public class DiCompositionRoot : DependencyContainerBase
{
    [SerializeField] [Required] private UiRoot _uiRoot;
    [SerializeField] [Required] private GameEcsEntryPoint _entryPoint;
    [SerializeField] [Required] private Camera _camera;
    [SerializeField] [Required] private GridStaticData _gridData;
    [SerializeField] [Required] private CombatStaticData _combatData;
    [SerializeField] [Required] private LevelingStaticData _levelingData;
    [SerializeField] [Required] private StaminaStaticData _staminaData;
    [SerializeField] [Required] private VfxStaticData _vfxData;

    protected override void ComposeDependencies(ContainerBuilder builder)
    {
        builder
            .RegisterIfNotNull(_entryPoint)
            .RegisterIfNotNull(_gridData)
            .RegisterIfNotNull(_combatData)
            .RegisterIfNotNull(_camera)
            .RegisterIfNotNull(_staminaData)
            .RegisterIfNotNull(_vfxData)
            ;

        builder
            .Register<GridService>()
            .Register<GameBusyService>()
            .Register<UnityTime>()
            ;

        builder
            .RegisterIfNotNull(_levelingData)
            .Register<RequiredExperienceService>()
            ;

        builder
            .RegisterIfNotNull(_uiRoot)
            ;
    }
}