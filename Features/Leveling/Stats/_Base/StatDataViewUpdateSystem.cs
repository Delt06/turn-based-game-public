using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using Features._Shared;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Leveling.Stats._Base
{
    public class StatDataViewUpdateSystem<TStatData> : IEcsRunSystem, IEcsInitSystem, IEcsDestroySystem
        where TStatData : struct, IStatData
    {
        private readonly EcsFilter _filter;
        private readonly EcsPool<StatUpgradeRequest<TStatData>> _requests;
        private readonly EcsReadOnlyPool<TStatData> _stats;
        private readonly PlayerStatView _view;

        [UsedImplicitly]
        public StatDataViewUpdateSystem(PlayerStatView view, EcsWorld world)
        {
            _view = view;
            _filter = world.FilterAndIncUpdateOf<TStatData>().Inc<PlayerTag>().End();
            _stats = world.GetPool<TStatData>().AsReadOnly();
            _requests = world.GetPool<StatUpgradeRequest<TStatData>>();
        }

        public void Destroy(EcsSystems systems)
        {
            _view.Clicked -= OnClicked;
        }

        public void Init(EcsSystems systems)
        {
            _view.Clicked += OnClicked;
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                _view.Display(_stats.Read(i).Level + 1);
            }
        }

        private void OnClicked() => _requests.AddNewEntity();
    }
}