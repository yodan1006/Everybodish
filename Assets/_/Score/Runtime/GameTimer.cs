using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    private float gameStartTime;
    private float stoppedTime;
    private bool hasStarted = false;
    private bool isStopped = false;
    public bool IsRunning => hasStarted && !isStopped;

    public float ElapsedTime
    {
        get
        {
            if (!hasStarted) return 0f;
            if (isStopped) return stoppedTime;
            return Time.time - gameStartTime;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Starts or restarts the timer.
    /// </summary>
    public void StartGameTimer()
    {
        gameStartTime = Time.time;
        stoppedTime = 0f;
        hasStarted = true;
        isStopped = false;
        Debug.Log("Game timer started.");
    }

    /// <summary>
    /// Stops the timer and freezes the time.
    /// </summary>
    public void StopGameTimer()
    {
        if (hasStarted && !isStopped)
        {
            stoppedTime = Time.time - gameStartTime;
            isStopped = true;
            Debug.Log($"Game timer stopped at {stoppedTime:F2} seconds.");
        }
    }

    /// <summary>
    /// Resets the timer to zero and stops it.
    /// </summary>
    public void ResetGameTimer()
    {
        gameStartTime = 0f;
        stoppedTime = 0f;
        hasStarted = false;
        isStopped = false;
        Debug.Log("Game timer reset.");
    }

    /// <summary>
    /// Returns current elapsed time.
    /// </summary>
    public float GetTimestamp()
    {
        return ElapsedTime;
    }
}
