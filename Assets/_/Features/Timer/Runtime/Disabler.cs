using UnityEngine;

namespace Timer.Runtime
{
    [DisallowMultipleComponent]
    public class Disabler : MonoBehaviour
    {
        #region Publics

        #endregion
        private float timeBeforeDisable = 0;

        #region Unity Api


        // Update is called once per frame
        private void Update()
        {
            timeBeforeDisable -= Time.deltaTime;
            if (timeBeforeDisable < 0)
            {
                gameObject.SetActive(false);
                enabled = false;
            }
        }

        #endregion


        #region Main Methods
        public void EnableForDuration(float duration)
        {
            gameObject.SetActive(true);
            enabled = true;
            timeBeforeDisable = duration;

        }
        #endregion


        #region Utils

        #endregion


        #region Private and Protected

        #endregion


    }
}
