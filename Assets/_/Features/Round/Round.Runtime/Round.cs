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
        private float cooldownTimeDelta = 0;
        public int roundDuration = 300;

        public UnityEvent OnRoundStart = new();
        public UnityEvent OnWarmupStarted = new();
        public UnityEvent OnWarmupFinished = new();
        public UnityEvent OnGameplayStarted = new();
        public UnityEvent OnGameplayFinished = new();
        public UnityEvent OnCooldownStarted = new();
        public UnityEvent OnCooldownFinished = new();
        public UnityEvent OnRoundEnd = new();
        public UnityEvent<bool> OnPlayerLifeStatus = new();
        private GameTimer gameTimer;
        private readonly Dictionary<int, PlayerInput> players = new();
        public float WarmupTimeDelta { get => warmupTimeDelta; }

        private RoundState state = RoundState.Warmup;

        private enum RoundState
        {
            Warmup,
            Gameplay,
            Cooldown,
            Ended
        }

        private void Awake()
        {
           DontDestroyManager.Instance.RegisterToDestroy(this.gameObject);
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
            OnGameplayStarted.AddListener(StartRound);
            OnWarmupStarted.AddListener(Warmup);
            OnRoundEnd.AddListener(EndRound);
            ChangeState(RoundState.Warmup);
        }

        private void Warmup()
        {

            foreach (var item in players.Values)
            {
                GlobalScoreEventSystem.RegisterScoreEvent(item.playerIndex, ScoreEventType.JoinedGame);
            }
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

            switch (state)
            {
                case RoundState.Warmup:
                    warmupTimeDelta -= Time.deltaTime;
                    if (WarmupTimeDelta < 0)
                    {
                        ChangeState(RoundState.Gameplay);
                    }
                    break;
                case RoundState.Gameplay:
                    if (gameTimer.GetTime() > roundDuration)
                    {
                        ChangeState(RoundState.Cooldown);

                    }
                    break;
                case RoundState.Cooldown:
                    cooldownTimeDelta -= Time.deltaTime;
                    if (cooldownTimeDelta < 0)
                    {
                        ChangeState(RoundState.Ended);
                    }
                    break;
                case RoundState.Ended:
                    Debug.LogError("This should not happen!");
                    break;
            }

        }

        private void ChangeState(RoundState newState)
        {
            Debug.Log("Round state is now " + newState.ToString());
            state = newState;
            switch (newState)
            {
                case RoundState.Warmup:
                    enabled = true;
                    OnRoundStart.Invoke();
                    OnWarmupStarted.Invoke();
                    break;
                case RoundState.Gameplay:
                    OnWarmupFinished.Invoke();
                    OnGameplayStarted.Invoke();
                    gameTimer.StartGameTimer();
                    break;
                case RoundState.Cooldown:
                    gameTimer.StopGameTimer();
                    OnGameplayFinished.Invoke();
                    OnCooldownStarted.Invoke();
                    break;
                case RoundState.Ended:
                    OnCooldownFinished.Invoke();
                    OnRoundEnd.Invoke();
                    enabled = false;
                    break;
            }
        }

        public void JoinRound(PlayerInput playerInput)
        {
            int playerIndex = playerInput.playerIndex;
            players.Add(playerIndex, playerInput);
            Debug.Log($"Player with index {playerIndex} joined Round. Player count : {players.Count}");
        }

        public void LeaveRound(PlayerInput playerInput)
        {
            players.Remove(playerInput.playerIndex);
        }

        private void OnEnable()
        {

            warmupTimeDelta = warmupTime;
            cooldownTimeDelta = cooldownTime;
            OnWarmupStarted.Invoke();
            GlobalScoreEventSystem.ResetAllScores();
        }

        private void OnDisable()
        {
            gameTimer.StopGameTimer();
            OnGameplayFinished.Invoke();
        }

        public List<PlayerInput> Players()
        {
            return players.Values.ToList<PlayerInput>();
        }
    }
}
