using Common;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Animations
{
    public class CharacterAnimator : MonoBehaviour
    {
        private static readonly int IsMovingId = Animator.StringToHash("IsMoving");
        private static readonly int AttackId = Animator.StringToHash("Attack");
        private static readonly int AttackProgressId = Animator.StringToHash("AttackProgress");
        private static readonly int DiedId = Animator.StringToHash("Died");
        private static readonly int DealtDamageId = Animator.StringToHash("DealtDamage");

        [SerializeField] [Required] private Animator _animator = default!;
        [SerializeField] [Required] private Transform _movingTransform = default!;
        [SerializeField] [Min(0f)] private float _velocityThreshold = 0.1f;
        [SerializeField] [Required] private Transform _rotatedTransform = default!;
        [SerializeField] [Min(0f)] private float _rotationSmoothTime = 0.1f;
        [SerializeField] private AnimationCurve _attackProgressCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        private Quaternion _angularVelocity;
        private Vector3 _previousPosition;

        public Quaternion TargetRotation { get; set; }


        public Animator Animator => _animator;

        private void LateUpdate()
        {
            var currentPosition = _movingTransform.position;
            var motion = currentPosition - _previousPosition;
            var deltaTime = Time.deltaTime;
            var velocity = motion / deltaTime;

            var isMoving = IsMoving(velocity);
            UpdateIsMoving(isMoving);
            UpdateRotation(isMoving, motion, deltaTime);

            _previousPosition = currentPosition;
        }

        private void OnEnable()
        {
            _previousPosition = _movingTransform.position;
            TargetRotation = _rotatedTransform.rotation;
        }

        private bool IsMoving(Vector3 velocity) => velocity.sqrMagnitude >= _velocityThreshold * _velocityThreshold;

        private void UpdateIsMoving(bool isMoving)
        {
            _animator.SetBool(IsMovingId, isMoving);
        }

        private void UpdateRotation(bool isMoving, Vector3 motion, float deltaTime)
        {
            var currentRotation = _rotatedTransform.rotation;
            if (isMoving)
                TargetRotation = Quaternion.LookRotation(motion, Vector3.up);
            _rotatedTransform.rotation = QuaternionUtil.SmoothDamp(currentRotation, TargetRotation,
                ref _angularVelocity, _rotationSmoothTime, deltaTime
            );
        }

        public void StartAttack()
        {
            _animator.SetTrigger(AttackId);
        }

        public void SetAttackProgress(float progress)
        {
            _animator.SetFloat(AttackProgressId, _attackProgressCurve.Evaluate(progress));
        }

        public void OnDied()
        {
            _animator.SetTrigger(DiedId);
        }

        public void OnDealtDamage()
        {
            _animator.SetBool(DealtDamageId, true);
        }
    }
}