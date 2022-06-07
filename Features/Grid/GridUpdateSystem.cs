using DELTation.LeoEcsExtensions.Components;
using DELTation.LeoEcsExtensions.Utilities;
using JetBrains.Annotations;
using Leopotam.EcsLite;

namespace Features.Grid
{
    public class GridUpdateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilter _dirtyFilter;
        private readonly IGridService _grid;
        private readonly EcsFilter _gridPositionsFilter;
        private readonly EcsReadOnlyPool<GridPosition> _positions;
        private readonly EcsWorld _world;

        [UsedImplicitly]
        public GridUpdateSystem(IGridService grid, EcsWorld world)
        {
            _world = world;
            _grid = grid;
            _dirtyFilter = world.Filter<GridDirtyTag>().End();
            _gridPositionsFilter = world.Filter<GridPosition>().End();
            _positions = world.GetPool<GridPosition>().AsReadOnly();
        }

        public void Init(EcsSystems systems)
        {
            Update();
        }

        public void Run(EcsSystems systems)
        {
            if (_dirtyFilter.IsEmpty()) return;
            Update();
        }

        private void Update()
        {
            _grid.Clear();

            foreach (var i in _gridPositionsFilter)
            {
                ref readonly var gridPosition = ref _positions.Read(i);
                _grid.PutEntityAtPosition(gridPosition, _world.PackEntityWithWorld(i));
            }
        }
    }
}