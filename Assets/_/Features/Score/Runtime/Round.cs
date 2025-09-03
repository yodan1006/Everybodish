using UnityEngine;
using UnityEngine.Events;

namespace Score.Runtime
{
    [RequireComponent(typeof(GameTimer))]
    [RequireComponent(typeof(GlobalScoreEventSystem))]
    [DisallowMultipleComponent]
    public class Round : MonoBehaviour
    {

        public static Round Instance;
        public int warmupTime = 10;
        public float warmupTimeDelta = 0;
        public int roundDuration = 300;

        public UnityEvent OnWarmupStarted;
        public UnityEvent OnWarmupFinished;
        public UnityEvent OnRoundStarted;
        public UnityEvent OnRoundFinished;
        private GameTimer gameTimer;
        private GlobalScoreEventSystem globalScoreEventSystem;


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
            if (warmupTimeDelta > 0)
            {
                warmupTimeDelta -= Time.deltaTime;
                if (warmupTimeDelta < 0)
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
