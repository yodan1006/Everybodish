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
        }
        #endregion



    }
}
