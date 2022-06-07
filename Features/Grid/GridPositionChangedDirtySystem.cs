using DELTation.LeoEcsExtensions.Components;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Grid
{
    [UsedImplicitly]
    public class GridPositionChangedDirtySystem : IEcsRunSystem
    {
        private readonly EcsPool<GridDirtyTag> _dirtyTags;
        private readonly EcsFilter _positionChangedEvent;

        public GridPositionChangedDirtySystem(EcsWorld world)
        {
            _positionChangedEvent = world.Filter<GridPositionChangedEvent>().End();
            _dirtyTags = world.GetPool<GridDirtyTag>();
        }

        public void Run(EcsSystems systems)
        {
            if (!_positionChangedEvent.IsEmpty())
                _dirtyTags.AddNewEntity();
        }
    }
}