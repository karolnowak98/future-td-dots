using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;
using GlassyCode.FutureTD.Core.Grid.Components;
using GlassyCode.FutureTD.Core.Input.Components;
using GlassyCode.FutureTD.Gameplay.Turrets.Components;
using Unity.Transforms;

namespace GlassyCode.FutureTD.Gameplay.Turrets.Systems
{
    public partial struct PlacingTurretSystem : ISystem
    {
        private bool isCreated;
        private Entity newTurret;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.RequireForUpdate<OffensiveTurretAsset>();
            state.RequireForUpdate<LmbClickInput>();
            state.RequireForUpdate<GridData>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var camera = Camera.main;
            if (camera is null) return;
            
            var lmbInput = SystemAPI.GetSingleton<LmbClickInput>();
            var mousePos = Mouse.current.position.ReadValue();
            
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

                        var offensiveTurretAsset = SystemAPI.GetSingleton<OffensiveTurretAsset>();

                        if (offensiveTurretAsset.Asset.IsCreated)
                        {
                            var data = offensiveTurretAsset.Asset.Value.OffensiveTurretsData[0];

                            if (!isCreated)
                            {
                                newTurret = state.EntityManager.Instantiate(data.TurretPrefab);
                                isCreated = true;
                            }
                            
                            state.EntityManager.AddComponentData(newTurret, new LocalTransform { Position = gridFieldValue.CenterWorldPosition });
                            state.EntityManager.AddComponentData(newTurret, data);
                            state.EntityManager.AddComponentData(newTurret, new OffensiveTurret
                            {
                                CurrentAttackRanges = data.BaseAttackRanges,
                                CurrentAttackSpeed = data.BaseAttackSpeed
                            });
                        }

                        Debug.Log(gridFieldValue.Index.x + ", " + gridFieldValue.Index.y);
                    }
                }
            }
        }

        public void OnDestroy(ref SystemState state)
        {
            if (isCreated)
            {
                state.EntityManager.DestroyEntity(newTurret);
            }
        }
    }
}