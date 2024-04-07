using GlassyCode.FutureTD.Gameplay.Turrets.Components;
using GlassyCode.FutureTD.Gameplay.Turrets.SO;
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

                if (configs is null)
                {
                    Debug.LogError(nameof(TurretsContainer) + " is null!");
                    return;
                }

                var entity = GetEntity(TransformUsageFlags.None);
                
                var spawnBuffer = AddBuffer<SpawnBuffer>(entity);
                
                foreach (var prefab in configs.OffensiveTurrets)
                {
                    var prefabBufferElement = new SpawnBuffer
                    {
                        Prefab = GetEntity(prefab, TransformUsageFlags.Dynamic)
                    };

                    spawnBuffer.Add(prefabBufferElement);
                }
            }
        }
    }
}