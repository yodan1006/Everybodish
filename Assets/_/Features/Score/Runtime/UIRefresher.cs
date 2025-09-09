using System;
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
            
        }

        // Update is called once per frame
        void Update()
            {
        
            }
        private void Awake()
        {
            timer = GetComponent<GameTimer>();
            round = GetComponent<Round>();
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
        #endregion


    }
}
