using Unity.Entities;

namespace GlassyCode.FutureTD.Gameplay.Global
{
    public struct Spawner : IComponentData
    {
        public Entity Prefab;
    }
}