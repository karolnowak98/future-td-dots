using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;
using GlassyCode.FutureTD.Core.Grid.Components;
using GlassyCode.FutureTD.Core.Input.Components;
using GlassyCode.FutureTD.Gameplay.Turrets.Components;
using Unity.Mathematics;
using Unity.Transforms;
using IJobEntity = Unity.Entities.IJobEntity;

namespace GlassyCode.FutureTD.Gameplay.Turrets.Systems
{
    public partial struct PlacingTurretSystem : ISystem
    {
        private bool _isCreated;
        private Entity _newTurret;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BeginInitializationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.RequireForUpdate<OffensiveTurretImmutableAsset>();
            state.RequireForUpdate<LmbClickInput>();
            state.RequireForUpdate<GridData>();
        }
        
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            // if (_isCreated)
            // {
            //     state.EntityManager.DestroyEntity(_newTurret);
            // }
        }

        public void OnUpdate(ref SystemState state)
        {
            if (!SystemAPI.GetSingleton<LmbClickInput>().Clicked) return;

            var camera = Camera.main;
            
            if (camera is null) return;
            
            var mousePos = Mouse.current.position.ReadValue();
            var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            var ray = camera.ScreenPointToRay(mousePos);
            var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            
            var raycastInput = new RaycastInput
            {
                //Colliding with layer with index 6 ("Ground")
                Start = ray.origin,
                End = ray.GetPoint(1000f),
                Filter = new CollisionFilter
                {
                    BelongsTo = 1u << 6, 
                    CollidesWith = 1u << 6, 
                    GroupIndex = 0
                }
            };

            if (!physicsWorld.CastRay(raycastInput, out var hit)) return;
            
            var gridData = SystemAPI.GetSingleton<GridData>();

            if (!gridData.IsWorldPosInGrid(hit.Position)) return;
            
            var gridField = gridData.GetGridFieldByWorldPos(hit.Position);
            
            if (!gridField.HasValue) return;
            if (gridField.Value.HasTurret) return;

            new PlaceTurretJob
            {
                Ecb = ecb,
                GridField = gridField.Value,
                HitPosition = hit.Position
            }.Schedule();
            
            /*gridData.SetGridField(gridField.Value.Index, new GridField
            {
                Index = gridField.Value.Index,
                CenterWorldPosition = gridField.Value.CenterWorldPosition,
                HasTurret = true
            });*/
        }
    }
    
    [BurstCompile]
    public partial struct PlaceTurretJob : IJobEntity
    {
        public EntityCommandBuffer Ecb;
        public GridField GridField;
        public float3 HitPosition;
        
        private void Execute(in DynamicBuffer<SpawnBuffer> turrets)
        {
            var newTurret = Ecb.Instantiate(turrets[0].Prefab);
            var position = new float3(GridField.CenterWorldPosition.x, HitPosition.y + 0.5f, GridField.CenterWorldPosition.z);
                    
            Ecb.SetComponent(newTurret, new LocalTransform
            {
                Position = position,
                Scale = 1
            });
        }
    }
}