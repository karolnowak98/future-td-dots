using GlassyCode.FutureTD.Gameplay.Turrets.Components;
using GlassyCode.FutureTD.Gameplay.Turrets.SO;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GlassyCode.FutureTD.Gameplay.Turrets.Authoring
{
    public class TurretAuthoring : MonoBehaviour
    {
        [field: SerializeField] public OffensiveTurretConfig Config { private set; get; }
        
        private class TurretAuthoringBaker : Baker<TurretAuthoring>
        {
            public override void Bake(TurretAuthoring authoring)
            {
                if (authoring.enabled is false) return;
                
                var config = authoring.Config;
                if (config is null)
                {
                    Debug.LogError($"Make sure {nameof(OffensiveTurretConfig)} is not empty at any Turret Authoring!");
                    return;
                }
                
                var turret = GetEntity(TransformUsageFlags.Renderable);

                AddMutableComponent(turret, config);
                AddImmutableComponent(turret, config);
            }

            private void AddImmutableComponent(Entity turret, OffensiveTurretConfig config)
            {
                AddComponent(turret, new OffensiveTurretMutable
                {
                    CurrentAttackRanges = config.BaseAttackRanges,
                    CurrentAttackSpeed = config.BaseAttackSpeed,
                    ProjectilePrefab = GetEntity(config.ProjectilePrefab, TransformUsageFlags.Dynamic)
                });
            }
            
            private void AddMutableComponent(Entity turret, OffensiveTurretConfig config)
            {
                using var builder = new BlobBuilder(Allocator.Temp);
                ref var towerData = ref builder.ConstructRoot<OffensiveTurretImmutable>();
                
                towerData.Name = config.Name;
                towerData.Element = config.Element;
                towerData.AttackType = config.AttackType;
                towerData.BaseAttackRanges = config.BaseAttackRanges;
                towerData.BaseAttackSpeed = config.BaseAttackSpeed;
                towerData.ProjectileSpeed = config.ProjectileSpeed;

                var blobAsset = builder.CreateBlobAssetReference<OffensiveTurretImmutable>(Allocator.Persistent);
                
                AddBlobAsset(ref blobAsset, out _);
                AddComponent(turret, new OffensiveTurretImmutableAsset
                {
                    Asset = blobAsset
                });
            }
        }
    }
}
