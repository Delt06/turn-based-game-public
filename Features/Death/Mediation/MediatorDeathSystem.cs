using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Death.Mediation
{
    [UsedImplicitly]
    public class MediatorDeathSystem : IEcsRunSystem
    {
        private readonly EcsFilter _filter;
        private readonly EcsReadOnlyPool<DeathMediatorRef> _mediators;

        public MediatorDeathSystem(EcsWorld world)
        {
            _filter = world.Filter<DeathMediatorRef>().Inc<GridEntityDestructionRequest>().End();
            _mediators = world.GetPool<DeathMediatorRef>().AsReadOnly();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                _mediators.Read(i).Mediator.OnDied();
            }
        }
    }
}