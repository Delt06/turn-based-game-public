namespace Features.Leveling
{
    public interface IRequiredExperienceService
    {
        float GetExperienceRequiredFor(int levelToAchieve);
        float GetExperienceRequiredForNextLevel(in ExperienceData experienceData);
    }
}