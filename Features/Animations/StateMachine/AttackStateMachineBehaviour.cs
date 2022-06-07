using Features.Combat.Attack.Mediation;
using UnityEngine;

namespace Features.Animations.StateMachine
{
    public class AttackStateMachineBehaviour : StateMachineBehaviour
    {
        private static readonly int DealtDamageId = Animator.StringToHash("DealtDamage");
        private IAttackMediator _attackMediator = default!;

        public void Construct(IAttackMediator attackMediator)
        {
            _attackMediator = attackMediator;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);
            _attackMediator.OnFinishedAttack();
            animator.SetBool(DealtDamageId, false);
        }
    }
}