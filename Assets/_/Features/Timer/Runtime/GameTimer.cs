using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Timer.Runtime
{
    [DisallowMultipleComponent]
    public class GameTimer : MonoBehaviour
    {
        public static GameTimer Instance { get; private set; }

        private bool hasStarted = false;
        private bool isStopped = false;
        public float currentTime = 0;
        public float urgency;
        [SerializeField]private AudioSource UrgencySound;

        public bool IsRunning => hasStarted && !isStopped;
        public bool IsStopped => isStopped;
        public bool IsStarted => hasStarted;

        public UnityEvent onTimerStarted = new();
        public UnityEvent onTimerStopped = new();
        public UnityEvent onTimerPaused = new();
        private bool hasPlayedUrgencySound;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogError("Replacing old Timer", this);
                Destroy(Instance);
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Update()
        {
            if (hasStarted && !isStopped)
            {
                currentTime += Time.deltaTime;
                if (!hasPlayedUrgencySound && currentTime >= urgency)
                {
                    UrgencySound.Play();
                    hasPlayedUrgencySound = true; // On ne joue le son qu'une fois
                }
            }

        }

        public void StartGameTimer()
        {
            currentTime = 0f;
            hasStarted = true;
            isStopped = false;
            hasPlayedUrgencySound = false;
            Debug.Log("Game timer started.");
            onTimerStarted?.Invoke();
        }

        public void StopGameTimer()
        {
            if (hasStarted && !isStopped)
            {
                isStopped = true;
                Debug.Log($"Game timer stopped at {currentTime:F2} seconds.");
                onTimerStopped?.Invoke();
            }
        }

        public void ResetGameTimer()
        {
            currentTime = 0f;
            hasStarted = false;
            isStopped = false;
            hasPlayedUrgencySound = false;
            Debug.Log("Game timer reset.");
        }

        public float GetTime() => currentTime;

        public string GetFormattedTime() => FormatTime(currentTime);

        public string GetFormattedTimeLeft(float targetTime) => FormatTime(targetTime - currentTime);

        public static string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
            float seconds = timeInSeconds % 60f;
            return $"{minutes:00}:{seconds:00.0}";
        }
    }
}