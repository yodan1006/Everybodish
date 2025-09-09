using System.Collections.Generic;
using ActionMap;
using Grab.Runtime;
using Machine.Runtime;
using MovePlayer.Runtime;
using PlayerLocomotion.Runtime;
using Score.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
        
        public static List<SpawnSystem> AllPlayers = new List<SpawnSystem>();


        private void Awake()
        {
            
            // ajout d'un systeme dont destroy pour le passage de scene
            
            DontDestroyOnLoad(this);
            //
            
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

        private void Start()
        {
            inputMap.Player.Disable();
        //     //inputMap.Lobby.Disable();
        }

        private void OnDestroy()
        {
            AllPlayers.Remove(this);
        }

        private void OnEnable()
        {
            inputMap.Enable();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SetupNewPlayer();
        }

        private void OnDisable()
        {
            inputMap.Disable();
            SceneManager.sceneLoaded -= OnSceneLoaded;
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
                ScoreEvent(ScoreEventType.PlayerDied);
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
            BindPlayerInput(inputMap.Lobby.selectSkin, GetComponentInChildren<SelectSkin>().OnChangeColor);
            BindPlayerInput(inputMap.Lobby.selectSkin, GetComponentInChildren<SelectSkin>().OnChangeModel);
            BindPlayerInput(inputMap.Lobby.validateSkin, GetComponentInChildren<SelectSkin>().OnValidateSkin);
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

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 0)
            {
                inputMap.Player.Disable();
                inputMap.Lobby.Enable();
            }
            else
            {
                inputMap.Lobby.Disable();
                inputMap.Player.Enable();
            }
        }
    }
}
