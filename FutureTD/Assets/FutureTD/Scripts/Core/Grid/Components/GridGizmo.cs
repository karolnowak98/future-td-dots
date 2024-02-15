using Unity.Entities;

namespace GlassyCode.FutureTD.Core.Grid.Components
{
    public struct GridGizmo : IComponentData
    {
        public bool DrawGridGizmo;
        public float GridHeightOffset;
        public float GridFieldDivider;
    }
}