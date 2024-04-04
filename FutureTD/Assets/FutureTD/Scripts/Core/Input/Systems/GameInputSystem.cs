using GlassyCode.FutureTD.Core.Input.Components;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GlassyCode.FutureTD.Core.Input.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup), OrderLast = true)]
    public partial class GameInputSystem : SystemBase, GameInput.IGamePlayActions
    {
        private Entity _entity;
        private NativeArray<bool> _selectChoices;
        private GameInput _gameInput;
        private Vector2 _moveCamera;
        private bool _lmbClick;
        
        public void OnMoveCamera(InputAction.CallbackContext ctx) => _moveCamera = ctx.ReadValue<Vector2>();
        public void OnLMB(InputAction.CallbackContext ctx) { if(ctx.started) _lmbClick = true; }
        public void OnSelect1(InputAction.CallbackContext ctx){ if(ctx.started) _selectChoices[0] = true; }
        public void OnSelect2(InputAction.CallbackContext ctx){ if(ctx.started) _selectChoices[1] = true; }
        public void OnSelect3(InputAction.CallbackContext ctx){ if(ctx.started) _selectChoices[2] = true; }
        public void OnSelect4(InputAction.CallbackContext ctx){ if(ctx.started) _selectChoices[3] = true; }
        public void OnSelect5(InputAction.CallbackContext ctx){ if(ctx.started) _selectChoices[4] = true; }
        public void OnSelect6(InputAction.CallbackContext ctx){ if(ctx.started) _selectChoices[5] = true; }
        public void OnSelect7(InputAction.CallbackContext ctx){ if(ctx.started) _selectChoices[6] = true; }
        public void OnSelect8(InputAction.CallbackContext ctx){ if(ctx.started) _selectChoices[7] = true; }
        public void OnSelect9(InputAction.CallbackContext ctx){ if(ctx.started) _selectChoices[8] = true; }
        
        protected override void OnCreate()
        {
            _entity = EntityManager.CreateEntity();
            _selectChoices = new NativeArray<bool>(9, Allocator.Persistent);

            EntityManager.AddComponentData(_entity, new MoveCameraInput());
            EntityManager.AddComponentData(_entity, new LmbClickInput());
            EntityManager.AddComponentData(_entity, new SelectInput
            {
                Values = _selectChoices
            });
            
            _gameInput = new GameInput();
            _gameInput.GamePlay.SetCallbacks(this);
        }
        
        protected override void OnDestroy()
        {
            //SystemAPI.GetSingleton<SelectInput>().Values.Dispose();
            _selectChoices.Dispose();
        }

        protected override void OnStartRunning()
        {
            _gameInput.Enable();
        }
        
        protected override void OnStopRunning()
        {
            _gameInput.Disable();
        }

        protected override void OnUpdate()
        {
            SystemAPI.SetSingleton(new MoveCameraInput {Value = _moveCamera});
            SystemAPI.SetSingleton(new LmbClickInput {Clicked = _lmbClick});
            SystemAPI.SetSingleton(new SelectInput {Values = _selectChoices});

            ResetButtons();
        }

        private void ResetButtons()
        {
            for (var i = 0; i < _selectChoices.Length; i++)
            {
                _selectChoices[i] = false;
            }
            
            _lmbClick = false;
        }
    }
}