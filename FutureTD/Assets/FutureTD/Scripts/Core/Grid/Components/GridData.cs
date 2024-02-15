using Unity.Entities;
using Unity.Mathematics;

namespace GlassyCode.FutureTD.Core.Grid.Components
{
    public struct GridData : IComponentData
    {
        public BlobAssetReference<GridFields> GridFields;
        public int FieldSize;
    }
    
    public struct GridFields
    {
        public BlobArray<GridField> Value;
    }
    
    public struct GridField
    {
        public int2 Index;
        public float3 CenterWorldPosition;
        public bool IsWalkable;
    }
}