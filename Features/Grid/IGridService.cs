using Leopotam.EcsLite;
using Unity.Mathematics;

namespace Features.Grid
{
    public interface IGridService
    {
        float3 GridToWorldPosition(in GridPosition position);
        GridPosition WorldToGridPosition(in float3 position);
        bool TryGetEntityAtPosition(in GridPosition position, out EcsPackedEntityWithWorld entity);
        void PutEntityAtPosition(in GridPosition position, EcsPackedEntityWithWorld entity);
        void Clear();
    }
}