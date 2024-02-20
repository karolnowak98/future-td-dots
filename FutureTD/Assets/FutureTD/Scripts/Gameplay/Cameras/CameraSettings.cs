using GlassyCode.FutureTD.Utils.SO;
using UnityEngine;

namespace GlassyCode.FutureTD.Gameplay.Cameras
{
    [CreateAssetMenu(fileName = "Camera Settings", menuName = "Settings/Camera")]
    public class CameraSettings : Settings
    {
        [field: SerializeField] public float CameraMoveSpeed { get; set; }
        [field: SerializeField] public float CameraSmoothness { get; set; }
    }
}