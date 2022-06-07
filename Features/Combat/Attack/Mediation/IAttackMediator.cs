using Leopotam.EcsLite;

namespace Features.Combat.Attack.Mediation
{
    public interface IAttackMediator
    {
        void StartAttack(EcsPackedEntityWithWorld attackedEntity, float duration, float normalizedAttackDealDamageTime);
        void OnFinishedAttack();
    }
}