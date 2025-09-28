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
        public void TeleportTo(Transform target)
        {
            // 1. Cache target position and rotation
            Vector3 targetPos = target.position;
            Quaternion targetRot = target.rotation;

            // 2. Deactivate player and ragdoll to avoid physics issues
            _animator.enabled = false;
            m_playerRoot.SetActive(false);
            m_ragdollRoot.SetActive(false);

            // 3. Move player root
            m_playerRoot.transform.SetPositionAndRotation(targetPos, targetRot);

            // 4. Move ragdoll root to match player root in world space
            m_ragdollRoot.transform.SetPositionAndRotation(m_playerHip.transform.position, targetRot);

            // 6. Reactivate objects
            m_playerRoot.SetActive(true);
            m_ragdollRoot.SetActive(true);
            _animator.enabled = true;
        }

        public void ReconnectCharacterControllerToRagdoll()
        {
            // 1. Cache the ragdoll's current world position and rotation
            Vector3 ragdollPos = m_ragdollRoot.transform.position;
            Quaternion ragdollRot = m_ragdollRoot.transform.rotation;

            // 2. Deactivate objects to prevent physics interference
            _animator.enabled = false;
            m_playerRoot.SetActive(false);
            m_ragdollRoot.SetActive(false);

            // 3. Teleport player root to ragdoll's current position
            m_playerRoot.transform.SetPositionAndRotation(ragdollPos, ragdollRot);

            // 4. Reset ragdoll to initial pose at the same position
            ConfigurableJointExtended configurableJointExtended = m_ragdollRoot.GetComponent<ConfigurableJointExtended>();
            m_ragdollRoot.transform.SetPositionAndRotation(ragdollPos, ragdollRot);

            // 5. Reconnect the configurable joint
            ConfigurableJoint configurableJoint = m_ragdollRoot.GetComponent<ConfigurableJoint>();
            if (configurableJoint == null)
            {
                configurableJoint = m_ragdollRoot.AddComponent<ConfigurableJoint>();
            }
            configurableJointExtended.Reconnect(_rootRigidBody, configurableJoint, m_playerHip);

            // 6. Reactivate objects
            m_playerRoot.SetActive(true);
            m_ragdollRoot.SetActive(true);
            _animator.enabled = true;
        }

    }
}
