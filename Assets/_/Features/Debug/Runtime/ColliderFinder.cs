using UnityEngine;

public class ColliderFinder : MonoBehaviour
{
    #region Publics

    #endregion


    #region Unity Api

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        Collider[] colliders = GetComponentsInChildren<Collider>(true);
        Debug.Log("Found colliders: " + colliders.Length);
        foreach (var col in colliders)
        {
            Debug.Log("Collider on: " + col.gameObject.name + " | Type: " + col.GetType());
        }
    }


    #endregion


    #region Main Methods

    #endregion


    #region Utils

    #endregion


    #region Private and Protected

    #endregion


}
