using UnityEngine;

namespace Score.Runtime
{
    public class UIRefresher : MonoBehaviour
    {
        #region Publics
        public GameObject clock;
        public GameObject player1;
        public GameObject player2;
        public GameObject player3;
        public GameObject player4;
        public GameObject pause;

        #endregion


        #region Unity Api

        private void OnEnable()
        {
            GlobalScoreEventSystem.Instance?.OnScoreEvent?.AddListener(RefreshUIPlayer);
            GlobalScoreEventSystem.Instance?.OnScoresChanged?.AddListener(RefreshUI);
        }

        private void RefreshUIPlayer(ScoreEvent arg0)
        {
        }

        private void RefreshUI()
        { }


        // Update is called once per frame
        private void Update()
        {
            needle.transform.rotation = Quaternion.Euler(0, 0, -( timer.currentTime / 60 * 360 - needleStartRotation));
        }
        private void Awake()
        {
            timer = GetComponent<GameTimer>();
            round = GetComponent<Round>();
            needle = GameObject.Find("NEEDLE");
            needleStartRotation = needle.transform.rotation.eulerAngles.z;
            round.OnRoundStarted.AddListener(OnRoundStarted);
            round.OnRoundFinished.AddListener(OnRoundFinished);
            round.OnWarmupStarted.AddListener(OnWarmupStarted);
            round.OnWarmupFinished.AddListener(OnWarmupFinished);
        }

        private void OnWarmupFinished()
        {

        }

        private void OnWarmupStarted()
        {

        }

        private void OnRoundFinished()
        {

        }

        private void OnRoundStarted()
        {

        }
        #endregion


        #region Main Methods

        #endregion


        #region Utils

        #endregion


        #region Private and Protected

        private GameTimer timer;
        private Round round;
        private GameObject needle;
        private float needleStartRotation;
        #endregion


    }
}
