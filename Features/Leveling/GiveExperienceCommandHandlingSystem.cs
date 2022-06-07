using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Leveling
{
    public class GiveExperienceCommandHandlingSystem : IEcsRunSystem
    {
        private readonly EcsReadOnlyPool<GiveExperienceCommand> _commands;
        private readonly EcsFilter _commandsFilter;
        private readonly EcsReadWritePool<ExperienceData> _experience;
        private readonly EcsPool<LevelUpEvent> _levelUpEvents;
        private readonly IRequiredExperienceService _requiredExperience;

        [UsedImplicitly]
        public GiveExperienceCommandHandlingSystem(IRequiredExperienceService requiredExperience, EcsWorld world)
        {
            _requiredExperience = requiredExperience;
            _commandsFilter = world.Filter<GiveExperienceCommand>().End();
            _commands = world.GetPool<GiveExperienceCommand>().AsReadOnly();
            _experience = world.GetPool<ExperienceData>().AsReadWrite();
            _levelUpEvents = world.GetPool<LevelUpEvent>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _commandsFilter)
            {
                ref readonly var giveExperienceCommand = ref _commands.Read(i);
                var entity = giveExperienceCommand.Entity;
                if (!entity.Unpack(out _, out var entityIdx)) continue;
                if (!_experience.Has(entityIdx)) continue;

                ref var experienceData = ref _experience.Modify(entityIdx);
                experienceData.Experience += giveExperienceCommand.Amount;
                ProcessLevelUps(ref experienceData, entity);
            }
        }

        private void ProcessLevelUps(ref ExperienceData experienceData, EcsPackedEntityWithWorld entity)
        {
            while (true)
            {
                var requiredForNextLevel = _requiredExperience.GetExperienceRequiredForNextLevel(experienceData);
                if (experienceData.Experience < requiredForNextLevel) break;

                experienceData.Experience -= requiredForNextLevel;
                experienceData.Level++;
                PostLevelUpEvent(entity);
            }
        }

        private void PostLevelUpEvent(EcsPackedEntityWithWorld entity)
        {
            _levelUpEvents.AddNewEntity().Entity = entity;
        }
    }
}