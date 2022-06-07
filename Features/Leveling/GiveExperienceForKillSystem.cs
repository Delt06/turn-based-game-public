using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Features.Combat;
using Features.Death;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Leveling
{
    public class GiveExperienceForKillSystem : IEcsRunSystem
    {
        private readonly LevelingStaticData _data;
        private readonly EcsFilter _destructionFilter;
        private readonly EcsPool<GiveExperienceCommand> _experienceCommands;
        private readonly EcsReadOnlyPool<KillerData> _killers;

        [UsedImplicitly]
        public GiveExperienceForKillSystem(LevelingStaticData data, EcsWorld world)
        {
            _data = data;
            _destructionFilter = world.Filter<GridEntityDestructionRequest>().Inc<KillerData>().End();
            _killers = world.GetPool<KillerData>().AsReadOnly();
            _experienceCommands = world.GetPool<GiveExperienceCommand>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _destructionFilter)
            {
                ref readonly var killerData = ref _killers.Read(i);
                ref var giveExperienceCommand = ref _experienceCommands.AddNewEntity();
                giveExperienceCommand.Amount = _data.ExperienceForEnemy;
                giveExperienceCommand.Entity = killerData.Killer;
            }
        }
    }
}