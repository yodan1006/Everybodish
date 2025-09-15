using System.Collections.Generic;
using Score.Runtime;
using Spawner.Runtime;
using Timer.Runtime;
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
        public int roundDuration = 300;

        public UnityEvent OnWarmupStarted;
        public UnityEvent OnWarmupFinished;
        public UnityEvent OnRoundStarted;
        public UnityEvent OnRoundFinished;
        private GameTimer gameTimer;
        public List<PlayerInput> playerList;

        public float WarmupTimeDelta { get => warmupTimeDelta; }

        private void Awake()
        {
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
            foreach (PlayerInput player in playerList)
            {
                if (player != null)
                {
                    player.GetComponent<SpawnSystem>().enabled = false;
                    player.actions.FindActionMap("Player").Disable();
                }
            }
            playerList.Clear();
        }

        private void StartRound()
        {
            foreach (PlayerInput player in playerList)
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
            playerList.Add(playerInput);
            GlobalScoreEventSystem.RegisterScoreEvent(playerInput.playerIndex,ScoreEventType.JoinedGame);
        }

        public void LeaveRound(PlayerInput playerInput)
        {
            playerList.Remove(playerInput);
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
    }
}
