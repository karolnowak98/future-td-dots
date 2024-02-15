using Unity.Entities;
using Unity.Mathematics;

namespace GlassyCode.FutureTD.Core.Input.Components
{
    public struct MoveCameraInput : IComponentData
    {
        public float2 Value;
    }
}