using Grab.Runtime;
using Machine.Runtime;
using PlayerLocomotion.Runtime;
using Skins.Runtime;
using Spawner.Runtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace InputMapManager.Runtime
{
    public class InputMapSwitcher : MonoBehaviour
    {
        #region private
        private PlayerInput playerInput;
        private SpawnSystem spawnSystem;
        private readonly Dictionary<InputAction, List<System.Action<CallbackContext>>> boundActions = new();
        #endregion

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            spawnSystem = GetComponent<SpawnSystem>();

            // Clone the InputActionAsset BEFORE anything uses it
            var clonedAsset = ScriptableObject.Instantiate(playerInput.actions);
            clonedAsset.devices = playerInput.devices;
            playerInput.actions = clonedAsset;
        }

        private void Start()
        {
            // Grab Selfdestruct and wire it
            var selfDestruct = playerInput.actions.FindAction("Selfdestruct");
            if (selfDestruct != null)
            {
                selfDestruct.started += ctx => spawnSystem.KillPlayer(ctx);
            }
            else
            {
                Debug.LogError("Could not find 'Selfdestruct' action in Player map!");
            }

            // Subscribe to external events
            spawnSystem.onNewClone.AddListener(RebindPlayerControls);
            spawnSystem.onCloneDestroy.AddListener(UnbindPlayerControls);
            LobbyManager.Instance.onLobbyActive.AddListener(LobbyActive);
        }

        private void RoundActive() => SetGameplayMap();

        private void LobbyActive(bool isActive)
        {
            if (isActive)
                SetLobbyMap();
            else
                DisableAllInputs();
        }

        private void UnbindPlayerControls(GameObject go) => UnbindPlayerControls();

        private void RebindPlayerControls(GameObject go) => BindPlayerControls(go);

        public void BindPlayerControls(GameObject ctx)
        {
            BindPlayerInput(playerInput.actions.FindAction("Grab"), ctx.GetComponentInChildren<AnimatedProximityGrabber>().TryGrabReleaseAction);
            //BindPlayerInput(playerInput.actions.FindAction("Grab"), ctx.GetComponentInChildren<AnimatedProximityGrabber>().OnHoldGrabAction);
            BindPlayerInput(playerInput.actions.FindAction("HeadButt"), ctx.GetComponentInChildren<Attack>().PlayAttack);
            BindPlayerInput(playerInput.actions.FindAction("Interact"), ctx.GetComponentInChildren<PlayerInteract>().OnUse);
            BindPlayerInput(playerInput.actions.FindAction("Interact"), ctx.GetComponentInChildren<PlayerInteract>().OnManualCook);
            BindPlayerInput(playerInput.actions.FindAction("Move"), ctx.GetComponentInChildren<CameraRelativeMovement>().OnMovement);
            BindPlayerInput(playerInput.actions.FindAction("Move"), ctx.GetComponentInChildren<CameraRelativeRotation>().OnMovement);
        }

        public void UnbindPlayerControls()
        {
            foreach (var kvp in boundActions)
            {
                var inputAction = kvp.Key;
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
            if (inputAction == null)
            {
                Debug.LogWarning("Tried to bind to a null InputAction!");
                return;
            }

            var cachedAction = new System.Action<CallbackContext>(ctx => action(ctx));

            if (!boundActions.ContainsKey(inputAction))
                boundActions[inputAction] = new List<System.Action<CallbackContext>>();

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
            Debug.Log("Enabled Lobby inputs");
            playerInput.actions.FindActionMap("Lobby")?.Enable();
            playerInput.actions.FindActionMap("Player")?.Disable();
        }

        public void SetGameplayMap()
        {
            Debug.Log("Enabled Game inputs");
            playerInput.actions.FindActionMap("Lobby")?.Disable();
            playerInput.actions.FindActionMap("Player")?.Enable();
        }

        public void SetResultMap()
        {
            Debug.Log("Enabled Result inputs");
            playerInput.actions.FindActionMap("Lobby")?.Enable();
            playerInput.actions.FindActionMap("Player")?.Disable();
        }

        public void DisableAllInputs()
        {
            Debug.Log("Disabled all inputs");
            playerInput.actions.FindActionMap("Lobby")?.Disable();
            playerInput.actions.FindActionMap("Player")?.Disable();
        }
    }
}