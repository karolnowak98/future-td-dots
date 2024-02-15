using Unity.Entities;

namespace GlassyCode.FutureTD.Core.Grid.Components
{
    public readonly partial struct GridAspect : IAspect
    {
        public readonly Entity Self;

        private readonly RefRO<GridData> _data;
        private readonly RefRO<GridGizmo> _gizmo;
        
        public int FieldSize => _data.ValueRO.FieldSize;
        public BlobAssetReference<GridFields> GridFields => _data.ValueRO.GridFields;
        public float GridFieldDivider => _gizmo.ValueRO.GridFieldDivider;
        public float GridHeightOffset => _gizmo.ValueRO.GridHeightOffset;
        public bool DrawGridGizmo => _gizmo.ValueRO.DrawGridGizmo;
    }
}