using GlassyCode.FutureTD.Core.Grid.Components;
using GlassyCode.FutureTD.Core.Grid.Scriptable;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Authoring;
using UnityEngine;

namespace GlassyCode.FutureTD.Core.Grid.Authoring
{
    public class GridAuthoring : MonoBehaviour
    {
        [field: SerializeField] public GridSettings GridSettings { get; private set; }
        [field: SerializeField] public PhysicsShapeAuthoring PhysicsShape { get; private set; }
        
        private class GridAuthoringBaker : Baker<GridAuthoring>
        {
            public override void Bake(GridAuthoring authoring)
            {
                if(authoring.enabled is false) return;

                var gridSettings = authoring.GridSettings;
                if (gridSettings is null)
                {
                    Debug.LogError($"Make sure {nameof(gridSettings)} is not empty at any Grid Authoring!");
                    return;
                }

                var physicsShape = authoring.PhysicsShape;
                if (physicsShape is null)
                {
                    Debug.LogError($"Make sure {nameof(physicsShape)} is not empty at any Grid Authoring!");
                    return;
                }
                
                var entity = GetEntity(TransformUsageFlags.None);
                
                var fieldSize = gridSettings.FieldSize;
                var halfOfFieldSize = fieldSize / 2f;
                var builder = new BlobBuilder(Allocator.Temp);

                physicsShape.GetPlaneProperties(out var center, out var size, out _);

                var xGridSize = (int)size.x * (int)size.x / fieldSize;
                var yGridSize = (int)size.y * (int)size.y / fieldSize;
                
                var gridSize = new int2(xGridSize, yGridSize);
                
                ref var gridFields = ref builder.ConstructRoot<GridFields>();
                var gridFieldsArray = builder.Allocate(ref gridFields.Array, gridSize.x * gridSize.y);

                if (gridSize.x < fieldSize || gridSize.y <fieldSize)
                {
                    Debug.LogError("FieldSize is bigger than whole plane scale.");
                    return;
                }

                var xSize = (int)size.x * (int)size.x / 2;
                var zSize = (int)size.y * (int)size.y / 2;
                
                var xPos = center.x - xSize + halfOfFieldSize;
                var zPos = center.z - zSize + halfOfFieldSize;

                for (var x = 0; x < gridSize.x; x++)
                {
                    for (var y = 0; y < gridSize.y; y++)
                    {
                        var index = x + y * gridSize.x;
                    
                        gridFieldsArray[index] = new GridField
                        {
                            Index = new int2(x, y), 
                            CenterWorldPosition = new float3(xPos, 1, zPos),
                            HasTurret = false
                        };
                    
                        zPos += fieldSize;
                        Debug.Log($"({zPos},{xPos})");
                    }

                    xPos += fieldSize;
                    zPos = center.z - zSize + halfOfFieldSize;
                }

                var blobReference = builder.CreateBlobAssetReference<GridFields>(Allocator.Persistent);
                builder.Dispose();
                
                AddBlobAsset(ref blobReference, out _);
                AddComponent(entity, new GridData
                {
                    GridFields = blobReference, 
                    FieldSize = fieldSize, 
                    HalfOfFieldSize = halfOfFieldSize
                });
            }
        }
    }
}