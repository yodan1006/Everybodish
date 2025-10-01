using ActiveRagdoll.Data;
using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class PlayerTeleporter : MonoBehaviour
    {
        [SerializeField] private GameObject m_playerRoot;
        [SerializeField] private GameObject m_ragdollRoot;

        private Rigidbody _rootRigidBody;
        private Animator _animator;
        private Rigidbody _ragdollRootRigidbody;
        private ConfigurableJointExtended _ragdollRootJointExtended;
        private JointSettings _jointSettings;
        private Vector3 rootOffset;

        private void Awake()
        {

            _rootRigidBody = m_playerRoot.GetComponent<Rigidbody>();
            _animator = m_playerRoot.GetComponentInChildren<Animator>();
            _ragdollRootRigidbody = m_ragdollRoot.GetComponentInChildren<Rigidbody>();
            // _ragdollRootJoint = m_ragdollRoot.GetComponentInChildren<ConfigurableJoint>();
            _ragdollRootJointExtended = m_ragdollRoot.GetComponentInChildren<ConfigurableJointExtended>();
            ConfigurableJoint ragdollRootJoint = _ragdollRootJointExtended.GetComponentInChildren<ConfigurableJoint>();
            _jointSettings = JointSettings.FromJoint(ragdollRootJoint, _ragdollRootJointExtended);
            rootOffset = m_ragdollRoot.transform.position - m_playerRoot.transform.position;
        }


        public void TeleportTo(Transform target)
        {
            target.GetPositionAndRotation(out Vector3 targetPos, out Quaternion targetRot);

            // Deactivate objects to prevent physics issues
            _animator.enabled = false;
            m_playerRoot.SetActive(false);
            m_ragdollRoot.SetActive(false);

            // Move player root
            m_playerRoot.transform.SetPositionAndRotation(targetPos, targetRot);

            // Move ragdoll so its root matches player root
            m_ragdollRoot.transform.position = m_playerRoot.transform.position + rootOffset;
            m_ragdollRoot.transform.rotation = targetRot;

            // Reactivate
            m_playerRoot.SetActive(true);
            m_ragdollRoot.SetActive(true);
            _animator.enabled = true;
        }

        public void ReconnectCharacterControllerToRagdoll()
        {
            // Get ragdoll root transform
          m_ragdollRoot.transform.GetPositionAndRotation(out Vector3 ragdollPos, out Quaternion ragdollRot);

            // Deactivate to prevent physics interference
            _animator.enabled = false;
            // m_playerRoot.SetActive(false);
            // m_ragdollRoot.SetActive(false);
            Debug.LogError("Test1");
            // Move player root so that its root position matches the ragdoll root
            m_playerRoot.transform.position = ragdollPos;
            Debug.LogError("Test1");
            //Re-add offset from original prefab to ragdoll root
            m_ragdollRoot.transform.position = m_playerRoot.transform.position + rootOffset;
            Debug.LogError("Test1");
            //Set ragdoll root rotation to player rotation before reconnect
            _ragdollRootJointExtended.SetJointRotation(m_playerRoot.transform.rotation);
            Debug.LogError("Test1");
            // Reconnect the configurable joint
            ConfigurableJointExtended configurableJointExtended = m_ragdollRoot.GetComponentInChildren<ConfigurableJointExtended>();
            ConfigurableJoint configurableJoint;
            if (!_ragdollRootJointExtended.TryGetComponent<ConfigurableJoint>(out configurableJoint))
            {
                configurableJoint = _ragdollRootJointExtended.gameObject.AddComponent<ConfigurableJoint>();
            }
            configurableJointExtended.Reconnect(_rootRigidBody, configurableJoint, m_playerRoot);
            Debug.LogError("Test1");
            _jointSettings.ApplyTo(configurableJoint, configurableJointExtended);
            //Set ragdoll rotation back to it's previous rotation
            _ragdollRootJointExtended.SetJointRotation( ragdollRot);
            // Reactivate
            m_playerRoot.SetActive(true);
            m_ragdollRoot.SetActive(true);
            _animator.enabled = true;
            _ragdollRootJointExtended.enabled = true;
            ConfigurableJointExtended[] joints = configurableJointExtended.GetComponentsInChildren<ConfigurableJointExtended>();
            configurableJointExtended.SyncToTargetRotation();
            foreach (var item in joints)
            {

                item.SyncToTargetRotation();
            }
            Debug.LogError("Test1");
        }

        public void DisconnectRoot()
        {
            ConfigurableJoint ragdollRootJoint = _ragdollRootJointExtended.GetComponentInChildren<ConfigurableJoint>();
            m_ragdollRoot.GetComponent<ConfigurableJointExtended>().enabled = false;
            Destroy(ragdollRootJoint);
        }
    }
}