using Features._Shared.Data;
using UnityEngine;

namespace Features.Leveling
{
    [CreateAssetMenu(menuName = AssetPath + "Leveling")]
    public class LevelingStaticData : StaticDataBase
    {
        [SerializeField] private AnimationCurve _requiredExperienceCurve = default!;
        [SerializeField] [Min(0f)] private float _experienceForEnemy = 50f;
        [SerializeField] [Min(0)] private int _statPointsPerLevel = 2;
        [SerializeField] private AnimationCurve _maxHealthProgression = default!;
        [SerializeField] private AnimationCurve _damageProgression = default!;
        [SerializeField] private AnimationCurve _maxStaminaProgression = default!;

        public float ExperienceForEnemy => _experienceForEnemy;

        public int StatPointsPerLevel => _statPointsPerLevel;

        public float GetMaxHealth(int level) => _maxHealthProgression.Evaluate(level);
        public float GetDamage(int level) => _damageProgression.Evaluate(level);

        public float GetRequiredExperienceForLevel(int level) => _requiredExperienceCurve.Evaluate(level);

        public float GetMaxStamina(int level) => _maxStaminaProgression.Evaluate(level);
    }
}