using Unity.Entities;
using Unity.Mathematics;

namespace GlassyCode.FutureTD.Gameplay.Cameras.Components
{
    public struct Camera : IComponentData
    {
        public float3 Position;
        public quaternion Rotation;
    }
    
    public struct CameraData : IComponentData
    {
        public float MoveSpeed;
    }
}