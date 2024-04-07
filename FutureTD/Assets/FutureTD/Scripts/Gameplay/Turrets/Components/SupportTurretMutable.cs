using GlassyCode.FutureTD.Gameplay.Global.Configs;
using GlassyCode.FutureTD.Gameplay.Turrets.SO;
using Unity.Entities;

namespace GlassyCode.FutureTD.Gameplay.Turrets.Components
{
    public struct SupportTurretMutable : IComponentData
    {
        
    }
    
    public struct SupportTurretImmutable : IBufferElementData
    {
        public TurretName Name;
        public ElementType Element;
    }
    
    public struct SupportTurretImmutableAsset : IComponentData
    {
        public BlobAssetReference<SupportTurretImmutable> Asset;
    }
}