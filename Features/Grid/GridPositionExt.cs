using Unity.Mathematics;

namespace Features.Grid
{
    public static class GridPositionExt
    {
        public static float3 ToFloat3(this GridPosition gridPosition)
        {
            var position = gridPosition.Position;
            return new float3(position.x, 0, position.y);
        }

        public static bool IsNeighborTo(this in GridPosition position, in GridPosition otherPosition)
        {
            var difference = position.Position - otherPosition.Position;
            var length = (int) math.round(math.lengthsq(difference));
            return length == 1;
        }
    }
}