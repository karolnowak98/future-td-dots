using System;
using System.Collections.Concurrent;
using Unity.Mathematics;
using UnityEngine;

namespace GlassyCode.FutureTD.Core.Grid.Managers
{
    public class GizmosManager: MonoBehaviour
    {
        [SerializeField] private bool _drawGridGizmo;
        [SerializeField] private float _gridHeightOffset = 0.5f;
        [SerializeField] private float _gridFieldDivider;
        
        private readonly ConcurrentQueue<(float3,float3)> _gizmosToDraw = new();

        public static GizmosManager Instance { get; private set; }
        public bool DrawGridGizmo => _drawGridGizmo;
        public float GridHeightOffset => _gridHeightOffset;
        public float GridFieldDivider => _gridFieldDivider;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            
            else
            {
                Instance = this;
            }
        }
        
        public void OnDrawGizmos()
        {
            if (!_drawGridGizmo)
            {
                return;
            }
            
            while (_gizmosToDraw.Count > 0)
            {
                _gizmosToDraw.TryDequeue(out var action);
                
                Gizmos.color = Color.white;
                Gizmos.DrawCube(action.Item1, action.Item2);
            }
        }
        
        public void EnqueueGizmo(float3 centerOfCube, float3 sizeOfCube)
        {
            _gizmosToDraw.Enqueue(new ValueTuple<float3, float3>(centerOfCube, sizeOfCube));
        }
    }
}