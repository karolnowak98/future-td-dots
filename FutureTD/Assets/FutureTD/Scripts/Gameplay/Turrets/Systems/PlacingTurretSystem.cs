using GlassyCode.FutureTD.Core.Grid.Components;
using GlassyCode.FutureTD.Core.Input.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GlassyCode.FutureTD.Gameplay.Turrets.Systems
{
    public partial struct PlacingTurretSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
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

                    if (gridData.WorldPosIsInGrid(hit.Position))
                    {
                        Debug.Log(hit.Position);
                    }
                }
                
                //findgridfield on the grid
                //placeturret in right gridfield
            }
        }
    }
}