using Unity.Mathematics;
using UnityEngine;

namespace GlassyCode.FutureTD.Gameplay.Cameras
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private CameraSettings _settings;
        [SerializeField] private Transform _camera;
        
        public static CameraManager Instance { get; private set; }
        public bool IsMainCameraReady { get; private set; }
        public Vector3 CameraPosition => _camera.position;
        public Quaternion CameraRotation => _camera.rotation;
        public float CameraMoveSpeed => _settings.CameraMoveSpeed;

        private void Awake()
        {
            if (Instance != null)
            {
                IsMainCameraReady = false;
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }
        
        public void PlaceMainCamera(float3 position)
        {
            _camera.position = position;
            IsMainCameraReady = true;
        }

        public void ResetCamera()
        {
            IsMainCameraReady = false;
        }

        public void SetCameraPosition(float3 position)
        {
            _camera.position = math.lerp(_camera.position, position, _settings.CameraSmoothness);
        }
        
        public void SetCameraRotation(quaternion rotation)
        {
            _camera.rotation = math.slerp(_camera.rotation, rotation, _settings.CameraSmoothness);
        }
    }
}