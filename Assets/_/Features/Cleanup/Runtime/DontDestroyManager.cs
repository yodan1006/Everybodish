using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DontDestroyManager : MonoBehaviour
{
    private static DontDestroyManager _instance;
    public static DontDestroyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find an existing instance in the scene
                _instance = FindAnyObjectByType<DontDestroyManager>();

                if (_instance == null)
                {
                    // If none exists, create a new GameObject and attach this component
                    GameObject singletonGO = new("DontDestroyManager");
                    _instance = singletonGO.AddComponent<DontDestroyManager>();
                }

                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private readonly List<GameObject> objectsToDestroy = new();
    private readonly List<GameObject> objectsToEnable = new();

    private void Awake()
    {
        // Enforce singleton pattern
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterToDestroy(GameObject go)
    {
        if (!objectsToDestroy.Contains(go))
        {
            DontDestroyOnLoad(go);
            objectsToDestroy.Add(go);
        }
    }

    public void RegisterToEnableOnDestroy(GameObject go)
    {
        if (!objectsToEnable.Contains(go))
        {
            DontDestroyOnLoad(go);
            objectsToEnable.Add(go);
        }
    }

    private void OnDestroy()
    {
        foreach (var go in objectsToDestroy)
        {
            if (go != null)
                Destroy(go);
        }

        foreach (var go in objectsToEnable)
        {
            if (go != null)
                go.SetActive(true);
        }

        objectsToDestroy.Clear();

        // Reset instance only if this is the one being destroyed
        if (_instance == this)
        {
            _instance = null;
        }
    }
}