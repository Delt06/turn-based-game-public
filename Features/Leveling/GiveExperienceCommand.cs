using Leopotam.EcsLite;

namespace Features.Leveling
{
    public struct GiveExperienceCommand
    {
        public EcsPackedEntityWithWorld Entity;
        public float Amount;
    }
}