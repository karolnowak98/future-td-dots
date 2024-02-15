using GlassyCode.FutureTD.Gameplay.Global.Configs;
using GlassyCode.FutureTD.Gameplay.Turrets.SO;
using Unity.Entities;

namespace GlassyCode.FutureTD.Gameplay.Turrets.Components
{
    public struct SupportTurret : IComponentData
    {
        
    }
    
    public struct SupportTurretAsset : IComponentData
    {
        public BlobAssetReference<SupportTurretBlob> Asset;
    }

    public struct SupportTurretBlob
    {
        public BlobArray<SupportTurretData> SupportTurretsData;
    }
    
    public struct SupportTurretData : IBufferElementData
    {
        public TurretName Name;
        public ElementType Element;
    }
}