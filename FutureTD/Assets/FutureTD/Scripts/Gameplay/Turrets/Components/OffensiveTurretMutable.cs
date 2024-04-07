using GlassyCode.FutureTD.Gameplay.Global.Configs;
using GlassyCode.FutureTD.Gameplay.Turrets.SO;
using Unity.Entities;
using Unity.Mathematics;

namespace GlassyCode.FutureTD.Gameplay.Turrets.Components
{
    public struct OffensiveTurretMutable : IComponentData
    {
        public uint2 CurrentAttackRanges;
        public float CurrentAttackSpeed;
        
        public Entity ProjectilePrefab;
    }
    
    public struct OffensiveTurretImmutable : IComponentData
    {
        public TurretName Name;
        public ElementType Element;
        public AttackType AttackType;
        public uint2 BaseAttackRanges;
        public float BaseAttackSpeed;
        public float ProjectileSpeed;
    }
    
    public struct OffensiveTurretImmutableAsset : IComponentData
    {
        public BlobAssetReference<OffensiveTurretImmutable> Asset;
    }
}