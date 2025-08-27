using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class PlayerTeleporter : MonoBehaviour
    {
        #region Publics
        [SerializeField] private GameObject m_playerRoot;
        [SerializeField] private GameObject m_ragdollRoot;

        public void TeleportTo(Transform transform)
        {
            m_playerRoot.transform.SetPositionAndRotation(transform.position, transform.rotation);
            m_ragdollRoot.transform.SetPositionAndRotation(transform.position, transform.rotation);
        }
        #endregion


        #region Unity Api

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }

        #endregion


        #region Main Methods

        #endregion


        #region Utils

        #endregion


        #region Private and Protected

        #endregion


    }
}
