using TMPro;
using UnityEngine;

namespace Score.Runtime
{
    [RequireComponent(typeof(GlobalScoreEventSystem))]
    public class ScoreRefresher : MonoBehaviour
    {
        private GlobalScoreEventSystem scoreSystem;
        [SerializeField] private TextMeshProUGUI player1Score;
        [SerializeField] private TextMeshProUGUI player2Score;
        [SerializeField] private TextMeshProUGUI player3Score;
        [SerializeField] private TextMeshProUGUI player4Score;
        private void Awake()
        {
            scoreSystem = GetComponent<GlobalScoreEventSystem>();
            scoreSystem.OnScoresChanged.AddListener(OnScoresChanged);
        }

        private void OnScoresChanged() {

        }
    }
}
