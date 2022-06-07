namespace Features.Combat.Health
{
    public static class HealthExt
    {
        public static bool IsDead(in this HealthData healthData) => healthData.Value <= 0f;
    }
}