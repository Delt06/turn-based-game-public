using DELTation.LeoEcsExtensions.Composition;
using DELTation.LeoEcsExtensions.Composition.Di;
using Features.Combat.Attack;
using Features.Combat.Damage;
using Features.Combat.Regeneration;
using Features.Death;
using Features.Death.Mediation;
using Features.Grid;
using Features.HealthBar;
using Features.InputProcessing;
using Features.Leveling;
using Features.Leveling.OnDamage;
using Features.Leveling.Stats;
using Features.Leveling.Stats.Damage;
using Features.Leveling.Stats.Health;
using Features.Leveling.Stats.Stamina;
using Features.Movement;
using Features.Stamina;
using Leopotam.EcsLite;

public class GameEcsEntryPoint : EcsEntryPoint
{
    protected override void PopulateSystems(EcsSystems systems, EcsWorld world)
    {
        systems
            .CreateAndAdd<GridRaycastSystem>()
            .CreateAndAdd<ClickHandlingSystem>()
            .OneFrame<CellClickEvent>()
            ;

        systems
            .CreateAndAdd<GridMovementSystem>()
            .OneFrame<MovementRequest>();

        systems
            .CreateAndAdd<StaminaAttackRequirementSystem>()
            .CreateAndAdd<GridAttackSystem>()
            .OneFrame<AttackRequest>()
            .CreateAndAdd<StaminaAttackConsumptionSystem>()
            .CreateAndAdd<SecondaryAttackSystem>()
            .CreateAndAdd<AttackMediatorStartSystem>()
            .OneFrame<AttackStartedEvent>();

        systems.CreateAndAdd<AttackDealDamageSystem>()
            .OneFrame<AttackDealDamageRequest>()
            .CreateAndAdd<FixedDamageIntakeSystem>()
            .CreateAndAdd<TakeDamageSystem>()
            .OneFrame<TakeDamageRequest>()
            .CreateAndAdd<AttackFinishSystem>()
            .OneFrame<AttackFinishEvent>()
            ;

        systems
            .CreateAndAdd<EntityDestructionDelaySystem>()
            .CreateAndAdd<MediatorDeathSystem>()
            .CreateAndAdd<HealthBarHideOnDeathSystem>()
            .CreateAndAdd<GiveExperienceForKillSystem>()
            .CreateAndAdd<GridEntityDestructionSystem>()
            .OneFrame<GridEntityDestructionRequest>()
            ;

        systems
            .CreateAndAdd<GiveExperienceOnDamageSystem>()
            .CreateAndAdd<GiveExperienceCommandHandlingSystem>()
            .OneFrame<GiveExperienceCommand>()
            .CreateAndAdd<GiveStatPointsForLevelUpSystem>()
            .OneFrame<LevelUpEvent>()
            .CreateAndAddFeature<HealthStatFeature>()
            .CreateAndAddFeature<DamageStatFeature>()
            .CreateAndAddFeature<StaminaStatFeature>()
            ;

        systems
            .CreateAndAdd<GridPositionSyncSystem>()
            .CreateAndAdd<GridPositionChangedDirtySystem>()
            .CreateAndAdd<GridUpdateSystem>()
            .OneFrame<GridDirtyTag>()
            .OneFrame<GridPositionChangedEvent>()
            ;

        systems
            .CreateAndAdd<HealthRegenerationSystem>()
            .CreateAndAdd<StaminaRegenerationSystem>()
            ;

        systems
            .CreateAndAdd<HealthBarDisplaySystem>()
            .CreateAndAdd<PlayerExperienceViewSystem>()
            .CreateAndAdd<PlayerStatsPointsViewSystem>()
            .CreateAndAdd<PlayerStaminaViewSystem>()
            ;

        systems
            .OneFrame<TookDamageEvent>()
            .OneFrameUpdateEvents()
            ;
    }
}