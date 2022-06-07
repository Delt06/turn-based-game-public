using System.Collections.Generic;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using Unity.Mathematics;

namespace Features.Grid
{
    public class GridService : IGridService
    {
        private readonly Dictionary<int2, EcsPackedEntityWithWorld> _grid =
            new Dictionary<int2, EcsPackedEntityWithWorld>();
        private readonly GridStaticData _gridData;

        [UsedImplicitly]
        public GridService(GridStaticData gridData) => _gridData = gridData;

        public float3 GridToWorldPosition(in GridPosition position) => position.ToFloat3() * _gridData.CellSize;

        public GridPosition WorldToGridPosition(in float3 position) =>
            new GridPosition
            {
                Position = (int2) math.round(position.xz / _gridData.CellSize),
            };

        public bool TryGetEntityAtPosition(in GridPosition position, out EcsPackedEntityWithWorld entity) =>
            _grid.TryGetValue(position.Position, out entity);

        public void Clear() => _grid.Clear();

        public void PutEntityAtPosition(in GridPosition position, EcsPackedEntityWithWorld entity)
        {
            _grid[position.Position] = entity;
        }
    }
}