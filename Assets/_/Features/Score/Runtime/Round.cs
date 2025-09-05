using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Score.Runtime
{
    [RequireComponent(typeof(GameTimer))]
    [RequireComponent(typeof(GlobalScoreEventSystem))]
    [DisallowMultipleComponent]
    public class Round : MonoBehaviour
    {

        public static Round Instance;
        public int warmupTime = 10;
        private float warmupTimeDelta = 0;
        public int roundDuration = 300;

        public UnityEvent OnWarmupStarted;
        public UnityEvent OnWarmupFinished;
        public UnityEvent OnRoundStarted;
        public UnityEvent OnRoundFinished;
        private GameTimer gameTimer;
        private GlobalScoreEventSystem globalScoreEventSystem;
        public List<PlayerInput> playerList;

        public float WarmupTimeDelta { get => warmupTimeDelta; }

        private void Awake()
        {
            gameTimer = GetComponent<GameTimer>();
            globalScoreEventSystem = GetComponent<GlobalScoreEventSystem>();
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.Log("More than one Round instanced! Replacing old round by new one.", this);
                Instance = this;
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
            playerList.Add(playerInput);
            globalScoreEventSystem.RegisterScoreEvent(playerInput.playerIndex, ScoreEventType.JoinedGame, 0);
        }

        private void OnEnable()
        {

            warmupTimeDelta = warmupTime;
            OnWarmupStarted.Invoke();
            globalScoreEventSystem.ResetAllScores();
        }

        private void OnDisable()
        {
            gameTimer.StopGameTimer();
        }
    }
}
