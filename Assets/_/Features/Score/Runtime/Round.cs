using UnityEngine;
using UnityEngine.Events;

namespace Score.Runtime
{
    [RequireComponent(typeof(GameTimer))]
    [RequireComponent(typeof(GlobalScoreEventSystem))]
    public class Round : MonoBehaviour
    {
        #region Publics
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
        #endregion


        #region Unity Api
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
                Debug.LogError("More than one Round instanced! If you want to access the round, use Round.Instance.", this);
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
                if (gameTimer.ElapsedTime > roundDuration)
                {

                    gameTimer.StopGameTimer();
                    OnRoundFinished.Invoke();
                }
            }
        }

        private void OnEnable()
        {

            warmupTimeDelta = warmupTime;
            OnWarmupStarted.Invoke();

        }

        private void OnDisable()
        {

        }

        #endregion


        #region Main Methods

        #endregion


        #region Utils

        #endregion


        #region Private and Protected

        #endregion


    }
}
