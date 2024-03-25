using System.Collections.Generic;
using GlassyCode.FutureTD.Gameplay.Turrets.Components;
using GlassyCode.FutureTD.Gameplay.Turrets.SO;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace GlassyCode.FutureTD.Gameplay.Turrets.Authoring
{
    public class TurretsAuthoring : MonoBehaviour
    {
        [field: SerializeField] public TurretsContainer Container { get; private set; }

        private class TurretsAuthoringBaker : Baker<TurretsAuthoring>
        {
            public override void Bake(TurretsAuthoring authoring)
            {
                var configs = authoring.Container;

                if (configs == null)
                {
                    Debug.LogError("Make sure that Turrets Configs is not empty in authoring.");
                    return;
                }

                var entity = GetEntity(TransformUsageFlags.None);

                AddOffensiveConfigs(entity, configs.OffensiveTurretsConfigs);
                AddSupportConfigs(entity, configs.SupportTurretsConfigs);
            }

            private void AddOffensiveConfigs(Entity entity, IReadOnlyList<OffensiveTurretConfig> configs)
            {
                using var bb = new BlobBuilder(Allocator.Temp);
                ref var blob = ref bb.ConstructRoot<OffensiveTurretBlob>();
                var data = bb.Allocate(ref blob.OffensiveTurretsData, configs.Count);

                for (var i = 0; i < configs.Count; i++)
                {
                    var currentConfig = configs[i];

                    data[i] = new OffensiveTurretData
                    {
                        TurretPrefab = GetEntity(currentConfig.Prefab, TransformUsageFlags.Renderable),
                        Name = currentConfig.Name,
                        Element = currentConfig.Element,
                        AttackType = currentConfig.AttackType,
                        BaseAttackRanges = currentConfig.BaseAttackRanges,
                        BaseAttackSpeed = currentConfig.BaseAttackSpeed,
                        ProjectilePrefab = GetEntity(currentConfig.ProjectilePrefab, TransformUsageFlags.Dynamic),
                        ProjectileSpeed = currentConfig.ProjectileSpeed
                    };
                }
                
                var offensiveTurretBlob = bb.CreateBlobAssetReference<OffensiveTurretBlob>(Allocator.Persistent);
                AddBlobAsset(ref offensiveTurretBlob, out _);
                AddComponent(entity, new OffensiveTurretAsset { Asset = offensiveTurretBlob });
            }

            private void AddSupportConfigs(Entity entity, IReadOnlyList<SupportTurretConfig> configs)
            {
                using var bb = new BlobBuilder(Allocator.Temp);
                ref var blob = ref bb.ConstructRoot<SupportTurretBlob>();
                var data = bb.Allocate(ref blob.SupportTurretsData, configs.Count);

                for (var i = 0; i < configs.Count; i++)
                {
                    var currentConfig = configs[i];

                    data[i] = new SupportTurretData
                    {
                        Name = currentConfig.Name,
                        Element = currentConfig.Element
                    };
                }
                
                var supportTurretBlob = bb.CreateBlobAssetReference<SupportTurretBlob>(Allocator.Persistent);
                AddBlobAsset(ref supportTurretBlob, out _);
                AddComponent(entity, new SupportTurretAsset { Asset = supportTurretBlob });
            }
        }
    }
}