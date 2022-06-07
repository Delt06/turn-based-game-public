using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Views;
using DG.Tweening;
using Features.Animations;
using Features.Animations.StateMachine;
using Features.Combat.Attack;
using Features.Vfx;
using Leopotam.EcsLite;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

namespace Features._Shared.Mediation
{
    public class CharacterMediator : MonoBehaviour, ICharacterMediator
    {
        [SerializeField] [Required] private EntityView _entityView = default!;
        [SerializeField] [Required] private Transform _movedTransform = default!;
        [SerializeField] [Required] private CharacterAnimator _characterAnimator = default!;
        [SerializeField] [Min(0f)] private float _destroyOnDeathDelay = 1f;

        private readonly object _attackTarget = new object();
        private VfxStaticData _vfxStaticData = default!;

        public void Construct(VfxStaticData vfxStaticData)
        {
            _vfxStaticData = vfxStaticData;
        }

        private void Awake()
        {
            var attackBehaviour = _characterAnimator.Animator.GetBehaviour<AttackStateMachineBehaviour>();
            attackBehaviour.Construct(this);
        }

        private void OnDestroy()
        {
            _movedTransform.DOKill();
        }

        public void StartAttack(EcsPackedEntityWithWorld attackedEntity, float duration,
            float normalizedAttackDealDamageTime)
        {
            SetAttackProgress(0f);
            _characterAnimator.StartAttack();
            _characterAnimator.TargetRotation = GetRotationTowardsEntity(attackedEntity);
            DOTween.Sequence()
                .SetId(_attackTarget)
                .Append(DOVirtual.Float(0f, 1f, duration, SetAttackProgress))
                .Join(DOVirtual.DelayedCall(duration * normalizedAttackDealDamageTime, DealDamage));
        }

        public void OnFinishedAttack()
        {
            DOTween.Kill(_attackTarget);
            if (_entityView.TryGetEntity(out var entity))
                entity.GetOrAdd<AttackFinishEvent>();
        }

        public void OnDied()
        {
            _characterAnimator.OnDied();
            DOVirtual.DelayedCall(_destroyOnDeathDelay, () =>
                {
                    var vfxPrefab = _vfxStaticData.DeathVfx;
                    Instantiate(vfxPrefab, transform.position, Quaternion.identity);
                    _entityView.Destroy();
                }
            );
        }

        public void Move(float3 targetPosition, float speed, Ease ease, TweenCallback onComplete)
        {
            var duration = math.distance(_movedTransform.position, targetPosition) /
                           speed;

            _movedTransform.DOKill();
            _movedTransform.DOMove(targetPosition, duration)
                .SetEase(ease)
                .SetId(this)
                .OnComplete(onComplete);
        }

        private Quaternion GetRotationTowardsEntity(EcsPackedEntityWithWorld entity)
        {
            var position = _movedTransform.position;
            Transform entityTransform = entity.GetOrAdd<UnityObjectData<Transform>>();
            var entityPosition = entityTransform.position;
            return Quaternion.LookRotation(entityPosition - position, Vector3.up);
        }

        private void SetAttackProgress(float progress) => _characterAnimator.SetAttackProgress(progress);

        private void DealDamage()
        {
            if (!_entityView.TryGetEntity(out var entity)) return;
            entity.GetOrAdd<AttackDealDamageRequest>();
            _characterAnimator.OnDealtDamage();
        }
    }
}