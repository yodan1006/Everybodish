using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
        private CharacterController _characterController;
        private Animator _animator;
        private void Awake()
        {
            _rootRigidBody = m_playerRoot.GetComponent<Rigidbody>();
            _characterController = m_playerRoot.GetComponentInChildren<CharacterController>();
            _animator = m_playerRoot.GetComponentInChildren<Animator>();
        }
        public void TeleportTo(Transform transform)
        {
            m_playerRoot.SetActive(false);
            m_ragdollRoot.SetActive(false);
            m_playerRoot.transform.SetPositionAndRotation(transform.position, transform.rotation);
            ConfigurableJointExtended configurableJointExtended = m_ragdollRoot.GetComponent<ConfigurableJointExtended>();
            m_ragdollRoot.transform.localPosition = configurableJointExtended.InitialLocalPosition;
            m_ragdollRoot.transform.localRotation = configurableJointExtended.InitialLocalRotation;
            m_playerRoot.SetActive(true);
            m_ragdollRoot.SetActive(true);
        }


        public void ReconnectCharacterControllerToRagdoll()
        {
            _animator.enabled = false;
            ConfigurableJoint configurableJoint = m_ragdollRoot.AddComponent<ConfigurableJoint>();
            ConfigurableJointExtended configurableJointExtended = m_ragdollRoot.GetComponent<ConfigurableJointExtended>();

            // 1. Cache ragdoll's current world pos & rot
            m_ragdollRoot.transform.GetPositionAndRotation(out Vector3 ragdollPos, out Quaternion ragdollRot);

            // 2. Move player root to ragdoll’s world position (player controller need to keep upright)
            _characterController.enabled = false; // disable first so it doesn’t block teleport
            _characterController.transform.position = ragdollPos;
            _characterController.enabled = true;

            // 3. Reset ragdoll relative to player
            m_ragdollRoot.transform.localPosition = configurableJointExtended.InitialLocalPosition;
            m_ragdollRoot.transform.localRotation = configurableJointExtended.InitialLocalRotation;

            // 4. Reconnect rigidbody + joints
            configurableJointExtended.Reconnect(_rootRigidBody, configurableJoint, m_playerHip);

            // 5. Restore ragdoll’s world pos & rot after joint reconnect
            m_ragdollRoot.transform.rotation = ragdollRot;

            _animator.enabled = true;
        }
    }
}
