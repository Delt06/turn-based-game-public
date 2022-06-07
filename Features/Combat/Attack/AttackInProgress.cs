using Leopotam.EcsLite;

namespace Features.Combat.Attack
{
    public struct AttackInProgress
    {
        public EcsPackedEntityWithWorld AttackedEntity;
        public bool Primary;
    }
}