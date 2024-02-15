using Unity.Entities;
using Unity.Mathematics;

namespace GlassyCode.FutureTD.Gameplay.Cameras.Components
{
    public readonly partial struct CameraAspect : IAspect
    {
        public readonly Entity Self;
        
        private readonly RefRO<CameraData> _cameraData;
        private readonly RefRW<Camera> _camera;
        
        public float MoveSpeed => _cameraData.ValueRO.MoveSpeed;
        
        public quaternion Rotation
        {
            get => _camera.ValueRO.Rotation;
            set => _camera.ValueRW.Rotation = value;
        }

        public float3 Position
        {
            get => _camera.ValueRO.Position;
            set => _camera.ValueRW.Position = value;
        }
    }
}