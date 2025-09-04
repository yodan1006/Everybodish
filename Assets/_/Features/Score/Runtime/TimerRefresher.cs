using TMPro;
using UnityEngine;

namespace Score.Runtime
{
    [RequireComponent(typeof(GameTimer))]
    [RequireComponent(typeof(Round))]
    public class TimerRefresher : MonoBehaviour
    {
        private GameTimer timer;
        private Round round;
        [SerializeField] private TextMeshProUGUI roundTimeleft;
        [SerializeField] private TextMeshProUGUI warmupTimeleft;
        private void Awake()
        {
            timer = GetComponent<GameTimer>();
            round = GetComponent<Round>();
            round.OnRoundStarted.AddListener(OnRoundStarted);
            round.OnRoundFinished.AddListener(OnRoundFinished);
            round.OnWarmupStarted.AddListener(OnWarmupStarted);
            round.OnWarmupFinished.AddListener(OnWarmupFinished);
        }

        private void OnWarmupStarted()
        {
            warmupTimeleft.enabled = true;
            roundTimeleft.enabled = false;
        }
        private void OnWarmupFinished()
        {
            warmupTimeleft.enabled = false;
        }

        private void OnRoundStarted()
        {
            roundTimeleft.enabled = true;
        }

        private void OnRoundFinished()
        {
            roundTimeleft.enabled = false;
        }

        private void Update()
        {
            if (warmupTimeleft.enabled)
            {
                warmupTimeleft.SetText(round.warmupTimeDelta.ToString("0"));
            }

            if (roundTimeleft.enabled)
            {
                roundTimeleft.SetText(timer.GetFormatedTimeLeft(round.roundDuration));
            }
        }
    }
}
