using GlassyCode.FutureTD.Gameplay.Global.Configs;
using Unity.Mathematics;
using UnityEngine;

namespace GlassyCode.FutureTD.Gameplay.Turrets.SO
{
    [CreateAssetMenu(menuName = "Configs/Offensive Turret", fileName = "Offensive Turret Config")]
    public class OffensiveTurretConfig : TurretConfig
    {
        [field: Header("Offensive statistics")]
        [field: SerializeField] 
        public AttackType AttackType { get; private set; }
        
        [field: SerializeField] 
        public uint2 BaseAttackRanges { get; private set; }
        
        [field: SerializeField, Tooltip("In seconds.")] 
        public float BaseAttackSpeed { get; private set; }
        
        [field: SerializeField] 
        public GameObject ProjectilePrefab { get; private set; }
        
        [field: SerializeField]
        public float ProjectileSpeed { get; private set; }
    }
}