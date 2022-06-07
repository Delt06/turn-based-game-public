using DELTation.LeoEcsExtensions.Utilities;
using Features._Shared;
using Features._Shared.Ui;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Leveling
{
    public class PlayerExperienceViewSystem : IEcsRunSystem
    {
        private readonly EcsReadOnlyPool<ExperienceData> _experience;
        private readonly EcsFilter _filter;
        private readonly PlayerExperienceView _playerExperienceView;
        private readonly IRequiredExperienceService _requiredExperience;

        [UsedImplicitly]
        public PlayerExperienceViewSystem(UiRoot uiRoot, IRequiredExperienceService requiredExperience, EcsWorld world)
        {
            _requiredExperience = requiredExperience;
            _playerExperienceView = uiRoot.PlayerExperienceView;
            _filter = world.FilterAndIncUpdateOf<ExperienceData>().Inc<PlayerTag>().End();
            _experience = world.GetPool<ExperienceData>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var experienceData = ref _experience.Read(i);
                var fill = experienceData.Experience /
                           _requiredExperience.GetExperienceRequiredForNextLevel(experienceData);
                var displayedLevel = experienceData.Level + 1;
                _playerExperienceView.Display(fill, displayedLevel);
            }
        }
    }
}