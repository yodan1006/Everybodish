using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class PlayerTeleporter : MonoBehaviour
    {
        [SerializeField] private GameObject m_playerRoot;
        [SerializeField] private GameObject m_ragdollRoot;
        private Rigidbody _rootRigidBody;
        private CharacterController _characterController;
        private void Awake()
        {
            _rootRigidBody = m_playerRoot.GetComponent<Rigidbody>();
            _characterController = m_playerRoot.GetComponentInChildren<CharacterController>();
        }
        public void TeleportTo(Transform transform)
        {
            m_playerRoot.SetActive(false);
            m_ragdollRoot.SetActive(false);
            m_playerRoot.transform.SetPositionAndRotation(transform.position, transform.rotation);
            Vector3 ragdollPosition = transform.position;

            m_ragdollRoot.transform.SetPositionAndRotation(transform.position, transform.rotation);
            ConfigurableJointExtended[] configurableJointExtendeds = m_ragdollRoot.GetComponentsInChildren<ConfigurableJointExtended>();
            m_playerRoot.SetActive(true);
            m_ragdollRoot.SetActive(true);
            Debug.LogError("Test");
        }


        public void ReconnectCharacterControllerToRagdoll()
        {
            //Find root joint
            ConfigurableJointExtended configurableJointExtended = m_ragdollRoot.GetComponentInChildren<ConfigurableJointExtended>();
            //get ragdoll position and rotation
            m_ragdollRoot.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
            //teleport player to current position of ragdoll
            _characterController.transform.position = position;
            //Set the position of the ragdoll back to it's original position correctly before reconnecting
            m_ragdollRoot.transform.SetPositionAndRotation(configurableJointExtended.target.transform.position, configurableJointExtended.target.transform.rotation);
            ReconnectRoot();
            //Reset the position to the start position to maintain the illusion nothing happend
            m_ragdollRoot.transform.SetPositionAndRotation(position, rotation);
        }
        private void ReconnectRoot()
        {
            ConfigurableJoint configurableJoint = m_ragdollRoot.AddComponent<ConfigurableJoint>();
            ConfigurableJointExtended configurableJointExtended = m_ragdollRoot.GetComponent<ConfigurableJointExtended>();
            configurableJointExtended.Reconnect(_rootRigidBody, configurableJoint);
        }
    }
}
