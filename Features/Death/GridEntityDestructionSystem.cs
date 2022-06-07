using DELTation.LeoEcsExtensions.Components;
using Features.Grid;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Death
{
    public class GridEntityDestructionSystem : IEcsRunSystem
    {
        private readonly EcsPool<GridDirtyTag> _dirtyTags;
        private readonly EcsFilter _filter;
        private readonly EcsWorld _world;

        [UsedImplicitly]
        public GridEntityDestructionSystem(EcsWorld world)
        {
            _world = world;
            _filter = world.Filter<GridEntityDestructionRequest>().End();
            _dirtyTags = world.GetPool<GridDirtyTag>();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _filter)
            {
                _world.DelEntity(i);
                MarkGridDirty();
            }
        }

        private void MarkGridDirty() => _dirtyTags.AddNewEntity();
    }
}