using Unity.Collections;
using Unity.Entities;

namespace GlassyCode.FutureTD.Core.Input.Components
{
    public struct SelectInput : IComponentData
    {
        public NativeArray<bool> Values;
    }
}