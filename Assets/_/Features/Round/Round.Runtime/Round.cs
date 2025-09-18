using System.Collections.Generic;
using System.Linq;
using Score.Runtime;
using Spawner.Runtime;
using Timer.Runtime;
using TransitionScene.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Round.Runtime
{
    [RequireComponent(typeof(GameTimer))]
    [RequireComponent(typeof(GlobalScoreEventSystem))]
    [DisallowMultipleComponent]
    public class RoundSystem : MonoBehaviour
    {

        public static RoundSystem Instance;
        public int warmupTime = 10;
        private float warmupTimeDelta = 0;
        public int cooldownTime = 5;
        private readonly float cooldownTimeDelta = 0;
        public int roundDuration = 300;

        public UnityEvent OnWarmupStarted = new();
        public UnityEvent OnWarmupFinished = new();
        public UnityEvent OnRoundStarted = new();
        public UnityEvent OnRoundFinished = new();
        public UnityEvent<bool> OnPlayerLifeStatus = new();
        private GameTimer gameTimer;
        private readonly Dictionary<int, PlayerInput> players = new();

        public float WarmupTimeDelta { get => warmupTimeDelta; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            gameTimer = GetComponent<GameTimer>();
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.Log("More than one Round instanced! Replacing old round by new one.", this);
                Instance = this;
            }
            OnRoundStarted.AddListener(StartRound);
            OnRoundFinished.AddListener(EndRound);
        }

        private void EndRound()
        {
            foreach (PlayerInput player in players.Values)
            {
                if (player != null)
                {
                    player.GetComponent<SpawnSystem>().enabled = false;
                    player.actions.FindActionMap("Player").Disable();
                }
            }
            players.Clear();
            SceneLoader loader = FindAnyObjectByType<SceneLoader>();
            if (loader != null)
            {
                loader.LoadSceneWithLoading(3);
            }
        }

        private void StartRound()
        {
            foreach (PlayerInput player in players.Values)
            {
                player.GetComponent<SpawnSystem>().enabled = true;
                player.actions.FindActionMap("Player").Enable();
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (WarmupTimeDelta > 0)
            {
                warmupTimeDelta -= Time.deltaTime;
                if (WarmupTimeDelta < 0)
                {
                    OnWarmupFinished.Invoke();
                    OnRoundStarted.Invoke();
                    gameTimer.StartGameTimer();
                }
            }
            else
            {
                if (gameTimer.GetTime() > roundDuration)
                {

                    gameTimer.StopGameTimer();
                    OnRoundFinished.Invoke();
                    enabled = false;
                }
            }
        }

        public void JoinRound(PlayerInput playerInput)
        {
            players.Add(playerInput.playerIndex, playerInput);
            GlobalScoreEventSystem.RegisterScoreEvent(playerInput.playerIndex, ScoreEventType.JoinedGame);  
            Debug.Log($"Player with index {playerInput.playerIndex} joined Round. Player count : {players.Count}");
        }

        public void LeaveRound(PlayerInput playerInput)
        {
            players.Remove(playerInput.playerIndex);
        }

        private void OnEnable()
        {

            warmupTimeDelta = warmupTime;
            OnWarmupStarted.Invoke();
            GlobalScoreEventSystem.ResetAllScores();
        }

        private void OnDisable()
        {
            gameTimer.StopGameTimer();
            OnRoundFinished.Invoke();
        }

        public List<PlayerInput> Players()
        {
            return players.Values.ToList<PlayerInput>();
        }
    }
}
