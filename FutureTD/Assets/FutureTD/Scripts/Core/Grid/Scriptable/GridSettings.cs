using GlassyCode.FutureTD.Utils.SO;
using Unity.Mathematics;
using UnityEngine;

namespace GlassyCode.FutureTD.Core.Grid.Scriptable
{
    [CreateAssetMenu(fileName = "Grid Settings", menuName = "Settings/Grid")]
    public class GridSettings : Settings
    {
        [field: SerializeField] public int2 GridSize { get; private set; }
        [field: SerializeField] public int FieldSize { get; private set; }
    }
}