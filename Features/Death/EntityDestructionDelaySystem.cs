using DELTation.LeoEcsExtensions.Components;
using Features._Shared.Busy;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Death
{
    public class EntityDestructionDelaySystem : IEcsRunSystem
    {
        private readonly IGameBusyService _busyService;
        private readonly EcsFilter _delayedFilter;
        private readonly EcsFilter _filter;
        private readonly EcsWorld _world;

        [UsedImplicitly]
        public EntityDestructionDelaySystem(IGameBusyService busyService, EcsWorld world)
        {
            _world = world;
            _busyService = busyService;
            _delayedFilter = world.Filter<DelayedGridEntityDestructionRequest>().End();
            _filter = world.Filter<GridEntityDestructionRequest>().End();
        }

        public void Run(EcsSystems systems)
        {
            if (_busyService.IsBusy)
                Swap<GridEntityDestructionRequest, DelayedGridEntityDestructionRequest>(_filter);
            else
                Swap<DelayedGridEntityDestructionRequest, GridEntityDestructionRequest>(_delayedFilter);
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private void Swap<TFrom, TTo>(EcsFilter filter) where TFrom : struct where TTo : struct
        {
            foreach (var i in filter)
            {
                _world.GetPool<TTo>().GetOrAdd(i);
                _world.GetPool<TFrom>().Del(i);
            }
        }
    }
}