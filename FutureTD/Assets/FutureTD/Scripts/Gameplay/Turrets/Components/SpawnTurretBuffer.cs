using GlassyCode.FutureTD.Gameplay.Turrets.SO;
using Unity.Entities;

namespace GlassyCode.FutureTD.Gameplay.Turrets.Components
{
    public struct SpawnBuffer : IBufferElementData
    {
        public TurretName Name;
        public Entity Prefab;
    }
}