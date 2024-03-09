using GlassyCode.FutureTD.Gameplay.Cameras.Components;
using Unity.Entities;
using UnityEngine;
using Camera = GlassyCode.FutureTD.Gameplay.Cameras.Components.Camera;

namespace GlassyCode.FutureTD.Gameplay.Cameras.Authoring
{
    public class CameraAuthoring : MonoBehaviour
    {
        private class MainCameraAuthoringBaker : Baker<CameraAuthoring>
        {
            public override void Bake(CameraAuthoring authoring)
            {
                if (CameraManager.Instance == null) return;
                
                var entity = GetEntity(TransformUsageFlags.None);
                
                AddComponent(entity, new Camera
                {
                    Position = CameraManager.Instance.CameraPosition,
                    Rotation = CameraManager.Instance.CameraRotation,
                });
                
                AddComponent(entity, new CameraData
                {
                    MoveSpeed = CameraManager.Instance.CameraMoveSpeed
                });
            }
        }
    }
}