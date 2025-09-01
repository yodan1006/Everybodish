using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }
    private bool hasStarted = false;
    private bool isStopped = false;
    private float currentTime = 0;
    public bool IsRunning => hasStarted && !isStopped;


    private void Update()
    {
        if (hasStarted && !isStopped)
        {
            currentTime = +Time.deltaTime;
        }
    }

    public float ElapsedTime
    {
        get
        {
            return currentTime;
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
        currentTime = 0f;
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
            isStopped = true;
            Debug.Log($"Game timer stopped at {currentTime:F2} seconds.");
        }
    }

    /// <summary>
    /// Resets the timer to zero and stops it.
    /// </summary>
    public void ResetGameTimer()
    {
        currentTime = 0f;
        hasStarted = false;
        isStopped = false;
        Debug.Log("Game timer reset.");
    }

    /// <summary>
    /// Returns current elapsed time.
    /// </summary>
    public float GetTime()
    {
        return ElapsedTime;
    }
}
