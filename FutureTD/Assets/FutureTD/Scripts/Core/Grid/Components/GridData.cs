using Unity.Entities;
using Unity.Mathematics;

namespace GlassyCode.FutureTD.Core.Grid.Components
{
    public struct GridData : IComponentData
    {
        public BlobAssetReference<GridFields> GridFields;
        public int FieldSize;
        public float HalfOfFieldSize;

        /*public void SetGridField(int2 index, GridField field)
        {
            if (index.x < 0 || index.x >= FieldSize || index.y < 0 || index.y >= FieldSize)
            {
                Debug.LogError("Invalid grid index");
                return;
            }

            var gridIndex = index.y * FieldSize + index.x;
            GridFields[gridIndex] = field;
        }*/
         
        public bool IsWorldPosInGrid(float3 worldPos)
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
        
        public GridField? GetGridFieldByWorldPos(float3 worldPos)
        {
            if (!GridFields.IsCreated) return null;
            if (!IsWorldPosInGrid(worldPos)) return null;

            ref var gridFields = ref GridFields.Value.Array;
            
            for (var i = 0; i < gridFields.Length; i++)
            {
                if (IsPosInGridField(worldPos, gridFields[i].CenterWorldPosition))
                {
                    return gridFields[i];
                }
            }

            return null;
        }

        private bool IsPosInGridField(float3 worldPos, float3 gridFieldCenter)
        {
            var leftBottomCorner = new float2(gridFieldCenter.x - HalfOfFieldSize, gridFieldCenter.z - HalfOfFieldSize);
            var rightTopCorner = new float2(gridFieldCenter.x + HalfOfFieldSize, gridFieldCenter.z + HalfOfFieldSize);

            var worldPosWithoutHeight = new float2(worldPos.x, worldPos.z);
            
            var isInside = worldPosWithoutHeight.x >= leftBottomCorner.x &&
                           worldPosWithoutHeight.x <= rightTopCorner.x &&
                           worldPosWithoutHeight.y >= leftBottomCorner.y &&
                           worldPosWithoutHeight.y <= rightTopCorner.y;

            return isInside;
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
        public bool HasTurret;
    }
}