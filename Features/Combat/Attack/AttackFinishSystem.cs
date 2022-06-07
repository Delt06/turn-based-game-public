using Features._Shared.Busy;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Combat.Attack
{
    public class AttackFinishSystem : IEcsRunSystem
    {
        private readonly EcsPool<AttackInProgress> _attacksInProgress;
        private readonly IGameBusyService _busyService;
        private readonly EcsFilter _filter;

        [UsedImplicitly]
        public AttackFinishSystem(IGameBusyService busyService, EcsWorld world)
        {
            _busyService = busyService;
            _filter = world.Filter<AttackInProgress>().Inc<AttackFinishEvent>().End();
            _attacksInProgress = world.GetPool<AttackInProgress>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                if (_attacksInProgress.Get(i).Primary)
                    _busyService.MakeFree();
                _attacksInProgress.Del(i);
            }
        }
    }
}