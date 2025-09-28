using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class PlayerTeleporter : MonoBehaviour
    {
        //player character root
        [SerializeField] private GameObject m_playerRoot;
        //player hip bone the configurable joint should copy the rotation of
        [SerializeField] private GameObject m_playerHip;
        //Ragdoll root located on the hip
        [SerializeField] private GameObject m_ragdollRoot;
        //Rigidbody placed on the player character root
        private Rigidbody _rootRigidBody;
        private Animator _animator;
        private void Awake()
        {
            _rootRigidBody = m_playerRoot.GetComponent<Rigidbody>();
            _animator = m_playerRoot.GetComponentInChildren<Animator>();
        }
        public void TeleportTo(Transform transform)
        {
            _animator.enabled = false;
            m_playerRoot.SetActive(false);
            m_ragdollRoot.SetActive(false);
            m_playerRoot.transform.SetPositionAndRotation(transform.position, transform.rotation);
            ConfigurableJointExtended configurableJointExtended = m_ragdollRoot.GetComponent<ConfigurableJointExtended>();
            m_ragdollRoot.transform.localPosition = configurableJointExtended.InitialLocalPosition;
            m_ragdollRoot.transform.localRotation = configurableJointExtended.InitialLocalRotation;
            m_playerRoot.SetActive(true);
            m_ragdollRoot.SetActive(true);
            _animator.enabled = true;
        }

        public void ReconnectCharacterControllerToRagdoll()
        {
            m_ragdollRoot.transform.GetPositionAndRotation(out Vector3 ragdollPos, out Quaternion ragdollRot);
            ConfigurableJoint configurableJoint = m_ragdollRoot.AddComponent<ConfigurableJoint>();
            ConfigurableJointExtended configurableJointExtended = m_ragdollRoot.GetComponent<ConfigurableJointExtended>();
            TeleportTo(m_playerRoot.transform);
            configurableJointExtended.Reconnect(_rootRigidBody, configurableJoint, m_playerHip);
            m_ragdollRoot.transform.SetPositionAndRotation(ragdollPos, ragdollRot);
        }
    }
}
