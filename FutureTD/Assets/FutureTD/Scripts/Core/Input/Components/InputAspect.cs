using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace GlassyCode.FutureTD.Core.Input.Components
{
    public readonly partial struct InputAspect : IAspect
    {
        public readonly Entity Self;
        
        private readonly RefRO<LmbClickInput> _lmbInput;
        private readonly RefRO<MoveCameraInput> _moveCameraInput;
        private readonly RefRO<SelectInput> _selectInput;
        
        public bool LmbInput => _lmbInput.ValueRO.Clicked;
        public float2 MoveCameraInput => _moveCameraInput.ValueRO.Value;
        public NativeArray<bool> Values => _selectInput.ValueRO.Values;
    }
}