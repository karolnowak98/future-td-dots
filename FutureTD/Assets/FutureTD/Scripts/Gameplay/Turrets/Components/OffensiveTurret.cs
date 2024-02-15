using GlassyCode.FutureTD.Gameplay.Global.Configs;
using GlassyCode.FutureTD.Gameplay.Turrets.SO;
using Unity.Entities;
using Unity.Mathematics;

namespace GlassyCode.FutureTD.Gameplay.Turrets.Components
{
    public struct OffensiveTurret : IComponentData
    {
        public uint2 CurrentAttackRanges;
        public float CurrentAttackSpeed;
    }
    
    public struct OffensiveTurretData : IComponentData
    {
        public TurretName Name;
        public ElementType Element;
        public AttackType AttackType;
        public uint2 BaseAttackRanges;
        public float BaseAttackSpeed;
        public Entity ProjectilePrefab;
        public float ProjectileSpeed;
    }
    
    public struct OffensiveTurretAsset : IComponentData
    {
        public BlobAssetReference<OffensiveTurretBlob> Asset;
    }
    
    public struct OffensiveTurretBlob
    {
        public BlobArray<OffensiveTurretData> OffensiveTurretsData;
    }
}