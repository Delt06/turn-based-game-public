using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Features.Combat.Damage;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Leveling.OnDamage
{
    public class GiveExperienceOnDamageSystem : IEcsRunSystem
    {
        private readonly EcsReadOnlyPool<TookDamageEvent> _events;
        private readonly EcsPool<ExperienceOnDamageData> _experienceOnDamage;
        private readonly EcsFilter _filter;
        private readonly EcsPool<GiveExperienceCommand> _giveExperienceCommands;

        [UsedImplicitly]
        public GiveExperienceOnDamageSystem(EcsWorld world)
        {
            _filter = world.Filter<TookDamageEvent>().End();
            _events = world.GetPool<TookDamageEvent>().AsReadOnly();
            _experienceOnDamage = world.GetPool<ExperienceOnDamageData>();
            _giveExperienceCommands = world.GetPool<GiveExperienceCommand>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                ref readonly var request = ref _events.Read(i).Request;
                var attackedEntity = request.AttackedEntity;
                if (!attackedEntity.Unpack(out _, out var attackedEntityIdx)) continue;
                if (!_experienceOnDamage.Has(attackedEntityIdx)) continue;

                ref var experienceOnDamageData = ref _experienceOnDamage.Get(attackedEntityIdx);
                ref var giveExperienceCommand = ref _giveExperienceCommands.AddNewEntity();
                giveExperienceCommand.Amount = experienceOnDamageData.Experience;
                giveExperienceCommand.Entity = request.Attacker;
            }
        }
    }
}