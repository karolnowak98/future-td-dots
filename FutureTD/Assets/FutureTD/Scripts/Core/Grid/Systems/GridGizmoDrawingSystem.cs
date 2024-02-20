using GlassyCode.FutureTD.Core.Grid.Components;
using GlassyCode.FutureTD.Core.Grid.Managers;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using GridAspect = GlassyCode.FutureTD.Core.Grid.Components.GridAspect;

namespace GlassyCode.FutureTD.Core.Grid.Systems
{
    [UpdateInGroup(typeof(TransformSystemGroup))]
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    public partial struct GridGizmoDrawingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GridGizmo>();
        }
        
        public void OnUpdate(ref SystemState state)
        {
            var entity = SystemAPI.GetSingletonEntity<GridGizmo>();
            var gridAspect = SystemAPI.GetAspect<GridAspect>(entity);

            if (!gridAspect.DrawGridGizmo)
                return;
            
            ref var gridFields = ref gridAspect.GridFields.Value;
            ref var gridFieldsArray  = ref gridFields.Array;
            var fieldSize = gridAspect.FieldSize;
            var divider = gridAspect.GridFieldDivider;
            var offset = gridAspect.GridHeightOffset;
            
            var squareSide = new float2(fieldSize / divider, fieldSize / divider);
            var sizeOfSquare = new float3(squareSide.x, 0, squareSide.y);
            
            for (var x = 0; x < gridFieldsArray.Length; x++)
            {
                var gridField = gridFieldsArray[x];
                var calculatedFieldPosition = new float3(gridField.CenterWorldPosition.x, 
                    offset, gridField.CenterWorldPosition.z);

                GizmosManager.Instance.EnqueueGizmo(calculatedFieldPosition, sizeOfSquare);
            }
        }
    }
}