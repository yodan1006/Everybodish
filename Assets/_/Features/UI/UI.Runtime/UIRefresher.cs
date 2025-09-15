using Animals.Data;
using Round.Runtime;
using Score.Runtime;
using Skins.Runtime;
using Timer.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Runtime
{
    public class UIRefresher : MonoBehaviour
    {
        #region Publics
        public GameObject clock;
        public GameObject[] playerUis;
        public GameObject[] playerTexts;
        public GameObject pause;
        public Image cadranMask;

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
            float currentTime = timer.currentTime;
            int roundDuration = round.roundDuration;
            float clockRange = 360f;
            float needleNormalizedOffset = currentTime / roundDuration;
            float needleAngle = needleNormalizedOffset * clockRange;
            needle.transform.rotation = Quaternion.Euler(0, 0, -needleAngle + needleStartRotation);
            cadranMask.fillAmount = needleNormalizedOffset;
        }
        private void Awake()
        {
            timer = GetComponent<GameTimer>();
            round = GetComponent<RoundSystem>();
            needle = GameObject.Find("NEEDLE");
            cadranMask = GameObject.Find("FILL").GetComponent<Image>();
            needleStartRotation = needle.transform.rotation.eulerAngles.z;
            round.OnRoundStarted.AddListener(OnRoundStarted);
            round.OnRoundFinished.AddListener(OnRoundFinished);
            round.OnWarmupStarted.AddListener(OnWarmupStarted);
            round.OnWarmupFinished.AddListener(OnWarmupFinished);
        }

        private void OnWarmupFinished()
        {
            int playerCount = round.playerList.Count;
            for (int i = 0; i < playerUis.Length; i++)
            {
                PlayerIcon playerIcon = playerUis[i].GetComponentInChildren<PlayerIcon>();
                if (i < playerCount)
                {
                    playerUis[i].SetActive(true);
                    playerTexts[i].SetActive(true);
                    PlayerInput player = round.playerList[i];
                    SelectSkin selectSkin = player.GetComponent<SelectSkin>();
                    AnimalType animalType = selectSkin.CurrentAnimalType();

                    if (playerIcon != null)
                    {
                        playerIcon.SetPlayerIcon(animalType);
                    }
                }
                else
                {
                    playerUis[i].SetActive(false);
                    playerTexts[i].SetActive(false);
                }

            }
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
        private RoundSystem round;
        private GameObject needle;
        private float needleStartRotation;
        #endregion


    }
}
