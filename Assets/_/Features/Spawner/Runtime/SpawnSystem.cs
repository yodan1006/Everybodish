using ActionMap;
using Grab.Runtime;
using MovePlayer.Runtime;
using PlayerMovement.Runtime;
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
        [SerializeField] private PlayerInputMap playerInputMap;
        private void Awake()
        {
            playerInputMap = new PlayerInputMap();
            playerInputMap.Player.Selfdestruct.started += KillPlayer;

        }


        private void OnEnable()
        {
            playerInputMap.Enable();
            SetupNewPlayer();
        }

        private void OnDisable()
        {
            playerInputMap.Disable();
            
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

        public void KillPlayer(CallbackContext callbackContext)
        {

            respawnTimeDelta = respawnTime;
            DestroyPlayer();
        }

        public void KillPlayerNoRespawn()
        {
            DestroyPlayer();
        }

        public void SetupNewPlayer()
        {
            InstantiatePlayer();
            BindPlayerControls();
        }

        public void InstantiatePlayer()
        {
            playerInstance = Instantiate(playerPrefab, transform);
        }

        public void BindPlayerControls()
        {
            //Get components
            Debug.Log("Binding inputs");
            BindPlayerInput(playerInputMap.Player.Grab, GetComponentInChildren<AnimatedProximityGrabber>().OnGrabAction);
            BindPlayerInput(playerInputMap.Player.HeadButt, GetComponentInChildren<Attack>().PlayAttack);
            BindPlayerInput(playerInputMap.Player.Interact, GetComponentInChildren<AnimatedProximityGrabber>().OnGrabAction);
            BindPlayerInput(playerInputMap.Player.Move, GetComponentInChildren<CameraRelativeMovement>().OnMovement);
            BindPlayerInput(playerInputMap.Player.Move, GetComponentInChildren<CameraRelativeRotation>().OnMovement);
        }

        public void UnBindPlayerControls()
        {
            //Get components
            Debug.Log("Binding inputs");
            UnBindPlayerInput(playerInputMap.Player.Grab, GetComponentInChildren<AnimatedProximityGrabber>().OnGrabAction);
            UnBindPlayerInput(playerInputMap.Player.HeadButt, GetComponentInChildren<Attack>().PlayAttack);
            UnBindPlayerInput(playerInputMap.Player.Interact, GetComponentInChildren<AnimatedProximityGrabber>().OnGrabAction);
            UnBindPlayerInput(playerInputMap.Player.Move, GetComponentInChildren<CameraRelativeMovement>().OnMovement);
            UnBindPlayerInput(playerInputMap.Player.Move, GetComponentInChildren<CameraRelativeRotation>().OnMovement);
        }

        public static void BindPlayerInput(InputAction inputAction, System.Action<CallbackContext> action)
        {
            inputAction.started += action;
            inputAction.performed += action;
            inputAction.canceled += action;
        }

        public static void UnBindPlayerInput(InputAction inputAction, System.Action<CallbackContext> action)
        {
            inputAction.started -= action;
            inputAction.performed -= action;
            inputAction.canceled -= action;
        }

        public void DestroyPlayer()
        {
            UnBindPlayerControls();
            Destroy(playerInstance);
            playerInstance = null;
        }
    }
}
