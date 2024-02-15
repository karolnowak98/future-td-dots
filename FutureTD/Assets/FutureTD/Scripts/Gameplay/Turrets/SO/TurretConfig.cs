using GlassyCode.FutureTD.Gameplay.Global.Configs;
using GlassyCode.FutureTD.Utils.SO;
using UnityEngine;

namespace GlassyCode.FutureTD.Gameplay.Turrets.SO
{
    public abstract class TurretConfig : Config
    {
        [field: Header("Base statistics")]
        [field: SerializeField] public TurretName Name { get; private set; }
        [field: SerializeField] public ElementType Element { get; private set; }
    }
    
    public enum TurretName
    {
        FireLord, 
        MeteoriteHole, 
        WaterSpring, 
        WaterElemental, 
        WindTurbine, 
        BigCyclone, 
        SprawlingTree, 
        ForestArcher
    }
}