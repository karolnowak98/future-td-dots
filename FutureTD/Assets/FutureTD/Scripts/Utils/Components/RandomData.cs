using Unity.Entities;
using Unity.Mathematics;

namespace GlassyCode.FutureTD.Utils.Components
{
    public struct RandomData : IComponentData
    {
        public Random Value;
    }
}