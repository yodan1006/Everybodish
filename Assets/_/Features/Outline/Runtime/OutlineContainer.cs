using Outline.Data;
using UnityEngine;

public class OutlineContainer : MonoBehaviour, IOutlineContainer
{
    #region Publics
    public GameObject outline;
    #endregion

    #region Main Methods
    public void EnableOutline(bool enabled)
    {
        outline.SetActive(enabled);
    }
    #endregion
}
