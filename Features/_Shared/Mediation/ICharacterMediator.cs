using DG.Tweening;
using Features.Combat.Attack.Mediation;
using Features.Death.Mediation;
using Unity.Mathematics;

namespace Features._Shared.Mediation
{
    public interface ICharacterMediator : IDeathMediator, IAttackMediator
    {
        void Move(float3 targetPosition, float speed, Ease ease, TweenCallback onComplete);
    }
}