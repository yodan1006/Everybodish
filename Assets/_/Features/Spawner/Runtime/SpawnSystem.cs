using System;
using System.Collections.Generic;
using ActionMap;
using Grab.Runtime;
using Machine.Runtime;
using MovePlayer.Runtime;
using PlayerLocomotion.Runtime;
using Score.Runtime;
using Skins.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Spawner.Runtime
{
    [RequireComponent(typeof(SelectSkin))]
    [RequireComponent(typeof(PlayerInput))]
    public class SpawnSystem : MonoBehaviour
    {

        public float respawnTime = 5;
        public float respawnTimeDelta;

        [SerializeField] private GameObject playerPrefab;
        private GameObject playerInstance;
        private PlayerInput playerInput;
        private PlayerInputMap inputMap;
        private readonly Dictionary<InputAction, List<System.Action<CallbackContext>>> boundActions = new();

        public static List<SpawnSystem> AllPlayers = new();
        public UnityEvent<bool> onPlayerLifeStatusChanged = new();
        public UnityEvent onPlayerQuit = new();

        private void Awake()
        {

            // ajout d'un systeme dont destroy pour le passage de scene

            DontDestroyManager.Instance.RegisterToDestroy(this.gameObject);

            playerInput = GetComponent<PlayerInput>();

            // Create wrapper from PlayerInput's actions
            inputMap = new PlayerInputMap
            {
                devices = playerInput.devices
            };
            inputMap.Player.Selfdestruct.started += ctx => KillPlayer(ctx);
            GetComponent<SelectSkin>().onSkinchanged.AddListener(RefreshPlayer);
        }

        private void RefreshPlayer()
        {
            RespawnPlayer();
        }

        private void OnDestroy()
        {
            AllPlayers.Remove(this);
        }

        private void OnEnable()
        {
     
            inputMap.Enable();
            SpawnPlayer();
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
                    SpawnPlayer();
                }
            }
        }

        public void KillPlayer()
        {
            if (playerInstance != null)
            {
                respawnTimeDelta = respawnTime;
                ScoreEvent(ScoreEventType.PlayerDied);
                DestroyPlayer();
                onPlayerLifeStatusChanged.Invoke(false);
            }
        }

        public void KillPlayer(CallbackContext callbackContext)
        {
            KillPlayer();
        }

        public void RespawnPlayer()
        {
            DestroyPlayer();
            SpawnPlayer();
        }

        public void RespawnPlayerAtLocation(Transform newTransform)
        {
            DestroyPlayer();
            transform.rotation = newTransform.rotation;
            transform.position = newTransform.position;
            SpawnPlayer();
        }

        public void KillPlayerNoRespawn()
        {
            DestroyPlayer();
        }

        public void CreatePlayer()
        {
            InstantiatePlayer();
            playerInstance.SetActive(true);
            BindPlayerControls();
            BindPlayerEvents();
        }

        public void SpawnPlayer()
        {
            CreatePlayer();
            onPlayerLifeStatusChanged.Invoke(true);
        }

        private void BindPlayerEvents()
        {
            GetComponentInChildren<PlayerStat>().onPlayerDied.AddListener(KillPlayer);
            GetComponentInChildren<PlayerInteract>().onScoreEvent.AddListener(ScoreEvent);
        }

        private void ScoreEvent(ScoreEventType eventType)
        {
            GlobalScoreEventSystem.RegisterScoreEvent(playerInput.playerIndex, eventType);
        }

        public void InstantiatePlayer()
        {
            playerInstance = Instantiate(playerPrefab, transform);
        }

        public void BindPlayerControls()
        {
            //Get components
            Debug.Log("Binding inputs");
            BindPlayerInput(inputMap.Player.Grab, GetComponentInChildren<AnimatedProximityGrabber>().TryGrabReleaseAction);
            BindPlayerInput(inputMap.Player.HeadButt, GetComponentInChildren<Attack>().PlayAttack);
            BindPlayerInput(inputMap.Player.Interact, GetComponentInChildren<PlayerInteract>().OnUse);
            BindPlayerInput(inputMap.Player.Interact, GetComponentInChildren<PlayerInteract>().OnManualCook);
            BindPlayerInput(inputMap.Player.Move, GetComponentInChildren<CameraRelativeMovement>().OnMovement);
            BindPlayerInput(inputMap.Player.Move, GetComponentInChildren<CameraRelativeRotation>().OnMovement);

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

        public void DestroyPlayer()
        {
            if (playerInstance != null)
            {
                UnbindPlayerControls();
                Destroy(playerInstance);
                playerInstance = null;
            }
        }

    }
}
