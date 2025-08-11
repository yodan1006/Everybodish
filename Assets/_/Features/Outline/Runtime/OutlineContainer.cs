using UnityEngine;

public class OutlineContainer : MonoBehaviour
{
    #region Publics
    public GameObject outline;
    public float disableTime = 0.5f;
    public float deltaDisable = 0;
    #endregion


    #region Unity Api

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (deltaDisable >= 0)
        {
            deltaDisable -= Time.deltaTime;
            if (deltaDisable < 0)
            {
                outline.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        outline.SetActive(false);
    }

    #endregion


    #region Main Methods
    public void EnableOutlineWithTimer()
    {
        if (deltaDisable <= 0)
        {
            outline.SetActive(true);
        }

        deltaDisable = disableTime;
    }
    #endregion


    #region Utils

    #endregion


    #region Private and Protected

    #endregion


}
