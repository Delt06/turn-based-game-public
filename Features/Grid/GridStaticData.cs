using Features._Shared.Data;
using UnityEngine;

namespace Features.Grid
{
    [CreateAssetMenu(menuName = AssetPath + "Grid")]
    public class GridStaticData : StaticDataBase
    {
        [Min(0f)]
        public float CellSize = 1f;

        public LayerMask CellsLayerMask;
        [Min(0f)]
        public float GridRaycastMaxDistance = 100f;
    }
}