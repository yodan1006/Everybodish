using System.Collections.Generic;
using ActionMap;
using Grab.Runtime;
using Machine.Runtime;
using PlayerLocomotion.Runtime;
using Spawner.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace InputMapManager.Runtime
{
    public class InputMapSwitcher : MonoBehaviour
    {
        #region privé
        private PlayerInput playerInput;
        private PlayerInputMap inputMap;
        private SpawnSystem spawnSystem;
        private readonly Dictionary<InputAction, List<System.Action<CallbackContext>>> boundActions = new();
        #endregion


        private void Start()
        {
            playerInput = gameObject.GetComponent<PlayerInput>();
        spawnSystem = gameObject.GetComponent<SpawnSystem>();

            // Create wrapper from PlayerInput's actions
            inputMap = new PlayerInputMap
            {
                devices = playerInput.devices
            };

            inputMap.Player.Selfdestruct.started += ctx => spawnSystem.KillPlayer(ctx);
            spawnSystem.onNewClone.AddListener(RebindPlayerControls);
            spawnSystem.onCloneDestroy.AddListener(UnbindPlayerControls);
        }

        private void UnbindPlayerControls(GameObject arg0)
        {
            UnbindPlayerControls();
        }

        private void RebindPlayerControls(GameObject ctx)
        {
            BindPlayerControls(ctx);
        }

        public void BindPlayerControls(GameObject ctx)
        {
            //Get components
            Debug.Log("Binding inputs");
            BindPlayerInput(inputMap.Player.Grab, ctx.GetComponentInChildren<AnimatedProximityGrabber>().TryGrabReleaseAction);
            BindPlayerInput(inputMap.Player.HeadButt, ctx.GetComponentInChildren<Attack>().PlayAttack);
            BindPlayerInput(inputMap.Player.Interact, ctx.GetComponentInChildren<PlayerInteract>().OnUse);
            BindPlayerInput(inputMap.Player.Interact, ctx.GetComponentInChildren<PlayerInteract>().OnManualCook);
            BindPlayerInput(inputMap.Player.Move, ctx.GetComponentInChildren<PlayerLocomotion.Runtime.CameraRelativeMovement>().OnMovement);
            BindPlayerInput(inputMap.Player.Move, ctx.GetComponentInChildren<CameraRelativeRotation>().OnMovement);

        }


        public void UnbindPlayerControls()
        {
            foreach (var kvp in boundActions)
            {
                InputAction inputAction = kvp.Key;
                foreach (var cachedAction in kvp.Value)
                {
                    inputAction.started -= cachedAction;
                    inputAction.canceled -= cachedAction;
                    inputAction.performed -= cachedAction;
                }
            }

            boundActions.Clear();
        }

        public void BindPlayerInput(InputAction inputAction, System.Action<CallbackContext> action)
        {
            System.Action<CallbackContext> cachedAction = ctx => action(ctx);

            if (!boundActions.ContainsKey(inputAction))
            {
                boundActions[inputAction] = new List<System.Action<CallbackContext>>();
            }

            boundActions[inputAction].Add(cachedAction);

            inputAction.started += cachedAction;
            inputAction.canceled += cachedAction;
            inputAction.performed += cachedAction;
        }

        public void UnbindPlayerInput(InputAction inputAction)
        {
            if (boundActions.TryGetValue(inputAction, out var callbacks))
            {
                foreach (var callback in callbacks)
                {
                    inputAction.started -= callback;
                    inputAction.canceled -= callback;
                    inputAction.performed -= callback;
                }

                boundActions.Remove(inputAction);
            }
        }

        public void SetLobbyMap()
        {
            inputMap.Lobby.Enable();
            inputMap.Player.Disable();
        }


        public void SetGameplayMap()
        {
            inputMap.Lobby.Disable();
            inputMap.Player.Enable();
        }

        public void SetResultMap()
        {
            inputMap.Lobby.Enable();
            inputMap.Player.Disable();
        }

        public void DisableAllInputs()
        {
            inputMap.Lobby.Disable();
            inputMap.Player.Disable();
        }
    }
}