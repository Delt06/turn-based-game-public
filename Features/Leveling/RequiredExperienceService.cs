using JetBrains.Annotations;

namespace Features.Leveling
{
    public class RequiredExperienceService : IRequiredExperienceService
    {
        private readonly LevelingStaticData _data;

        [UsedImplicitly]
        public RequiredExperienceService(LevelingStaticData data) => _data = data;

        public float GetExperienceRequiredFor(int levelToAchieve) =>
            _data.GetRequiredExperienceForLevel(levelToAchieve);

        public float GetExperienceRequiredForNextLevel(in ExperienceData experienceData) =>
            GetExperienceRequiredFor(experienceData.Level + 1);
    }
}