using Leopotam.EcsLite;

namespace Features.Combat.Damage
{
    public struct TakeDamageRequest
    {
        public float Damage;
        public EcsPackedEntityWithWorld AttackedEntity;
        public EcsPackedEntityWithWorld Attacker;
    }
}