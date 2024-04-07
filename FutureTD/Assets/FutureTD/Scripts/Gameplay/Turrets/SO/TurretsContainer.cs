using GlassyCode.FutureTD.Utils.SO;
using UnityEngine;

namespace GlassyCode.FutureTD.Gameplay.Turrets.SO
{
    [CreateAssetMenu(menuName = "Containers/Turrets", fileName = "Turrets Container")]
    public class TurretsContainer : Container
    {
        [field: SerializeField] public GameObject[] OffensiveTurrets { get; private set; }
        [field: SerializeField] public GameObject[] SupportTurretsConfigs { get; private set; }
    }
}