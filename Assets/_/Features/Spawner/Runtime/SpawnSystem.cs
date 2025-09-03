using System.Collections.Generic;
using ActionMap;
using Grab.Runtime;
using Machine.Runtime;
using MovePlayer.Runtime;
using PlayerLocomotion.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Spawner.Runtime
{
    [RequireComponent(typeof(PlayerInput))]
    public class SpawnSystem : MonoBehaviour
    {
        public float respawnTime = 5;
        public float respawnTimeDelta;

        [SerializeField] private GameObject playerPrefab;
        private GameObject playerInstance;
        private PlayerInput playerInput;
        private PlayerInputMap inputMap;
        private readonly Dictionary<InputAction, System.Action<CallbackContext>> boundActions = new();

        private void Awake()
        {

            playerInput = GetComponent<PlayerInput>();

            // Create wrapper from PlayerInput's actions
            inputMap = new PlayerInputMap
            {
                devices = playerInput.devices
            };
            inputMap.Player.Selfdestruct.started += ctx => KillPlayer(ctx);
            inputMap.Player.Selfdestruct.canceled += ctx => KillPlayer(ctx);
            inputMap.Player.Selfdestruct.performed += ctx => KillPlayer(ctx);
        }


        private void OnEnable()
        {
            inputMap.Enable();
            SetupNewPlayer();
        }

        private void OnDisable()
        {
            inputMap.Disable();

            DestroyPlayer();
        }

        // Update is called once per frame
        protected void Update()
        {
            if (respawnTimeDelta > 0)
            {
                respawnTimeDelta -= Time.deltaTime;
                if (respawnTimeDelta <= 0)
                {
                    SetupNewPlayer();
                }
            }
        }

        public void KillPlayer()
        {
            if (playerInstance != null)
            {
                respawnTimeDelta = respawnTime;
                DestroyPlayer();
            }
        }

        public void KillPlayer(CallbackContext callbackContext)
        {
            KillPlayer();
        }

        public void KillPlayerNoRespawn()
        {
            DestroyPlayer();
        }

        public void SetupNewPlayer()
        {
            InstantiatePlayer();
            BindPlayerControls();
            BindPlayerEvents();
        }

        private void BindPlayerEvents()
        {
            GetComponentInChildren<PlayerStat>().onPlayerDied.AddListener(KillPlayer);
        }

        public void InstantiatePlayer()
        {
            playerInstance = Instantiate(playerPrefab, transform);
        }

        public void BindPlayerControls()
        {
            //Get components
            Debug.Log("Binding inputs");
            BindPlayerInput(inputMap.Player.Grab, GetComponentInChildren<AnimatedProximityGrabber>().OnGrabAction);
            BindPlayerInput(inputMap.Player.Release, GetComponentInChildren<AnimatedProximityGrabber>().OnRelease);
            BindPlayerInput(inputMap.Player.HeadButt, GetComponentInChildren<Attack>().PlayAttack);
            BindPlayerInput(inputMap.Player.Interact, GetComponentInChildren<PlayerInteract>().OnUse);
            BindPlayerInput(inputMap.Player.Move, GetComponentInChildren<CameraRelativeMovement>().OnMovement);
            BindPlayerInput(inputMap.Player.Move, GetComponentInChildren<CameraRelativeRotation>().OnMovement);
        }

        public void UnBindPlayerControls()
        {
            //Get components
            Debug.Log("Binding inputs");
            foreach (InputAction inputAction in boundActions.Keys)
            {
                UnBindPlayerInput(inputAction);
            }
            boundActions.Clear();
        }

        public void BindPlayerInput(InputAction inputAction, System.Action<CallbackContext> action)
        {
            System.Action<CallbackContext> cachedAction = ctx => action(ctx);

            boundActions[inputAction] = cachedAction;
            inputAction.started += cachedAction;
            inputAction.canceled += cachedAction;
            inputAction.performed += cachedAction;
        }

        public void UnBindPlayerInput(InputAction inputAction)
        {
            if (boundActions.TryGetValue(inputAction, out var cachedAction))
            {
                inputAction.started -= cachedAction;
                inputAction.canceled -= cachedAction;
                inputAction.performed -= cachedAction;
            }
        }

        public void DestroyPlayer()
        {
            UnBindPlayerControls();
            Destroy(playerInstance);
            playerInstance = null;
        }
    }
}
