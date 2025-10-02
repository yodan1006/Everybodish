using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class PlayerTeleporter : MonoBehaviour
    {
        [SerializeField] private GameObject m_playerRoot;
        [SerializeField] private GameObject m_playerHip;
        [SerializeField] private GameObject m_ragdollRoot;
        [SerializeField] private GameObject m_ragdollHip;

        private Rigidbody _rootRigidBody;
        private Animator _animator;

        private void Awake()
        {
            _rootRigidBody = m_playerRoot.GetComponent<Rigidbody>();
            _animator = m_playerRoot.GetComponentInChildren<Animator>();
        }

        public void TeleportTo(Transform target)
        {
            Vector3 targetPos = target.position;
            Quaternion targetRot = target.rotation;

            // Deactivate objects to prevent physics issues
            _animator.enabled = false;
            m_playerRoot.SetActive(false);
            m_ragdollRoot.SetActive(false);

            // Move player root
            m_playerRoot.transform.SetPositionAndRotation(targetPos, targetRot);

            // Move ragdoll so its hip matches player hip
            m_ragdollRoot.transform.position = m_playerHip.transform.position;
            m_ragdollRoot.transform.rotation = targetRot;

            // Reactivate
            m_playerRoot.SetActive(true);
            m_ragdollRoot.SetActive(true);
            _animator.enabled = true;
        }

        public void ReconnectCharacterControllerToRagdoll()
        {
            // Get ragdoll hip transform
            Transform ragdollHip = m_ragdollRoot.transform; // Replace with actual ragdoll hip if different

            // Deactivate to prevent physics interference
            _animator.enabled = false;
            m_playerRoot.SetActive(false);
           // m_playerRoot.transform.rotation = m_ragdollRoot.transform.rotation;
             m_playerRoot.SetActive(false);
             m_ragdollRoot.SetActive(false);
            // Move player root so that its root position matches the ragdoll root
            //Re-add offset from original prefab to ragdoll root
            m_ragdollRoot.transform.position = m_playerRoot.transform.position + rootOffset;
            //Set ragdoll root rotation to player rotation before reconnect
            _ragdollRootJointExtended.SetJointRotation(m_playerRoot.transform.rotation);
            // Reconnect the configurable joint
            ConfigurableJointExtended configurableJointExtended = m_ragdollRoot.GetComponent<ConfigurableJointExtended>();
            ConfigurableJoint configurableJoint = m_ragdollRoot.AddComponent<ConfigurableJoint>();
            
 
            configurableJointExtended.Reconnect(_rootRigidBody, configurableJoint, m_playerHip);

            // Reactivate
            m_playerRoot.SetActive(true);
            m_ragdollRoot.SetActive(true);
            _animator.enabled = true;

            ConfigurableJointExtended[] joints = configurableJointExtended.GetComponentsInChildren<ConfigurableJointExtended>();
            configurableJointExtended.SyncToTargetRotation();
            foreach (var item in joints)
            {
                item.SyncToTargetRotation();
            }
        }
    }
}