using Unity.Entities;
using Unity.Mathematics;

namespace GlassyCode.FutureTD.Core.Grid.Components
{
    public struct GridData : IComponentData
    {
        public BlobAssetReference<GridFields> GridFields;
        public int FieldSize;
        public float HalfOfFieldSize;
        
        public bool WorldPosIsInGrid(float3 worldPos)
        {
            if (!GridFields.IsCreated) return false;

            var firstFieldPos = GridFields.Value.FirstElement.CenterWorldPosition;
            var lastFieldPos = GridFields.Value.LastElement.CenterWorldPosition;

            //If world pos exists in grid
            return worldPos.x >= firstFieldPos.x - HalfOfFieldSize 
                   && worldPos.x <= lastFieldPos.x + HalfOfFieldSize 
                   && worldPos.z >= firstFieldPos.z - HalfOfFieldSize
                   && worldPos.z <= lastFieldPos.z + HalfOfFieldSize;
        }
    }
    
    public struct GridFields
    {
        public BlobArray<GridField> Array;
        
        public GridField FirstElement => Array.Length > 0 ? Array[0] : default;
        public GridField LastElement => Array.Length > 0 ? Array[^1] : default;
    }

    public struct GridField
    {
        public int2 Index;
        public float3 CenterWorldPosition;
        public bool IsWalkable;
    }
}