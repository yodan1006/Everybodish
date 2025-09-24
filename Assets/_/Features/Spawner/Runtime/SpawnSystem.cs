using System.Collections.Generic;
using Machine.Runtime;
using MovePlayer.Runtime;
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
        private float respawnTimeDelta;

        [SerializeField] private GameObject playerPrefab;
        private GameObject playerInstance;
        private PlayerInput playerInput;

        public static List<SpawnSystem> AllPlayers = new();

        public UnityEvent<bool> onPlayerLifeStatusChanged = new();
        public UnityEvent onPlayerQuit = new();
        public UnityEvent<GameObject> onNewClone = new();
        public UnityEvent<GameObject> onCloneDestroy = new();

        private void Awake()
        {
            DontDestroyManager.Instance.RegisterToDestroy(gameObject);

            playerInput = GetComponent<PlayerInput>();
            GetComponent<SelectSkin>().onSkinchanged.AddListener(() => RespawnPlayer());
        }

        private void OnDestroy()
        {
            AllPlayers.Remove(this);
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void Update()
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

        public void KillPlayer(CallbackContext context)
        {
            KillPlayer();
        }

        public void RespawnPlayer(Transform newTransform = null)
        {
            DestroyPlayer();

            if (newTransform != null)
            {
                transform.position = newTransform.position;
                transform.rotation = newTransform.rotation;
            }

            SpawnPlayer();
        }

        public void KillPlayerNoRespawn()
        {
            DestroyPlayer();
        }

        private void SpawnPlayer()
        {
            if (playerInstance == null)
            {
                playerInstance = Instantiate(playerPrefab, transform);
                playerInstance.SetActive(true);
                BindPlayerEvents();
                onNewClone.Invoke(playerInstance);
                onPlayerLifeStatusChanged.Invoke(true);
            }
            else
            {
                Debug.LogError("Attempted to spawn player when one already exists.");
            }
        }

        private void BindPlayerEvents()
        {
            var stat = playerInstance.GetComponentInChildren<PlayerStat>();
            var interact = playerInstance.GetComponentInChildren<PlayerInteract>();

            if (stat != null)
                stat.onPlayerDied.AddListener(KillPlayer);
            if (interact != null)
                interact.onScoreEvent.AddListener(ScoreEvent);
        }

        private void ScoreEvent(ScoreEventType eventType)
        {
            GlobalScoreEventSystem.RegisterScoreEvent(playerInput.playerIndex, eventType);
        }

        private void DestroyPlayer()
        {
            if (playerInstance != null)
            {
                onCloneDestroy.Invoke(playerInstance);
                Destroy(playerInstance);
                playerInstance = null;
            }
        }
    }
}