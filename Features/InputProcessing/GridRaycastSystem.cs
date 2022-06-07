using DELTation.LeoEcsExtensions.Components;
using Features.Grid;
using JetBrains.Annotations;
using Leopotam.EcsLite;
using UnityEngine;

namespace Features.InputProcessing
{
    public class GridRaycastSystem : IEcsRunSystem
    {
        private readonly Camera _camera;
        private readonly EcsPool<CellClickEvent> _clicks;
        private readonly IGridService _grid;
        private readonly GridStaticData _gridData;

        [UsedImplicitly]
        public GridRaycastSystem(GridStaticData gridData, Camera camera, IGridService grid, EcsWorld world)
        {
            _gridData = gridData;
            _camera = camera;
            _grid = grid;
            _clicks = world.GetPool<CellClickEvent>();
        }

        public void Run(EcsSystems systems)
        {
            if (!Input.GetMouseButtonDown(0)) return;

            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, _gridData.GridRaycastMaxDistance, _gridData.CellsLayerMask,
                QueryTriggerInteraction.Ignore
            )) return;

            ref var cellClickEvent = ref _clicks.AddNewEntity();
            cellClickEvent.Position = _grid.WorldToGridPosition(hit.point);
        }
    }
}