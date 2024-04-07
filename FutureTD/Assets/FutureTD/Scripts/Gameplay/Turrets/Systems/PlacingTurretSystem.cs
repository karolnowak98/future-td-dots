using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;
using GlassyCode.FutureTD.Core.Grid.Components;
using GlassyCode.FutureTD.Core.Input.Components;
using GlassyCode.FutureTD.Gameplay.Global;
using GlassyCode.FutureTD.Gameplay.Turrets.Components;
using Unity.Transforms;

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

        public void OnUpdate(ref SystemState state)
        {
            var camera = Camera.main;
            if (camera is null) return;
            
            var lmbInput = SystemAPI.GetSingleton<LmbClickInput>();
            var mousePos = Mouse.current.position.ReadValue();
            var ecb = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            
            if (lmbInput.Clicked)
            {
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

                if(physicsWorld.CastRay(raycastInput, out var hit))
                {
                    var gridData = SystemAPI.GetSingleton<GridData>();

                    if (gridData.IsWorldPosInGrid(hit.Position))
                    {
                        var gridField = gridData.GetGridFieldByWorldPos(hit.Position);
                        if (!gridField.HasValue) return;

                        var gridFieldValue = gridField.Value;
                        
                        if (SystemAPI.TryGetSingletonBuffer(out DynamicBuffer<SpawnBuffer> turrets))
                        {
                            _newTurret = ecb.Instantiate(turrets[0].Prefab);
                    
                            ecb.AddComponent(_newTurret, new LocalTransform
                            {
                                Position = gridFieldValue.CenterWorldPosition, 
                                Scale = 1
                            });
                        }
                    }
                }
            }
        }

        public void OnDestroy(ref SystemState state)
        {
            // if (_isCreated)
            // {
            //     state.EntityManager.DestroyEntity(_newTurret);
            // }
        }
    }
}