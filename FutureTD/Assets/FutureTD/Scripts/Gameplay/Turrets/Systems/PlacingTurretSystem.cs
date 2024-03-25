using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;
using GlassyCode.FutureTD.Core.Grid.Components;
using GlassyCode.FutureTD.Core.Input.Components;
using GlassyCode.FutureTD.Gameplay.Turrets.Components;

namespace GlassyCode.FutureTD.Gameplay.Turrets.Systems
{
    public partial struct PlacingTurretSystem : ISystem
    {
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

                        var offensiveTurretAsset = SystemAPI.GetSingleton<OffensiveTurretAsset>();

                        if (offensiveTurretAsset.Asset.IsCreated)
                        {
                            var data = offensiveTurretAsset.Asset.Value.OffensiveTurretsData[0];
                            
                            var entity = state.EntityManager.CreateEntity();

                            state.EntityManager.AddComponentData(entity, data);
                            state.EntityManager.AddComponentData(entity, new OffensiveTurret
                            {
                                CurrentAttackRanges = data.BaseAttackRanges,
                                CurrentAttackSpeed = data.BaseAttackSpeed
                            });

                            var newTurret = state.EntityManager.Instantiate(data.TurretPrefab);
                            state.EntityManager.SetComponentData(newTurret, new Translation { Value = spawnPrefab.Position });
                        }
                        
                        //TODO placeturret in right gridfield

                        Debug.Log(gridField.Value.Index.x + ", " + gridField.Value.Index.y);
                    }
                }
            }
        }
    }
}