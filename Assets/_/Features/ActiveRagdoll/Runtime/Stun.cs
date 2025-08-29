using Grab.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ActiveRagdoll.Runtime
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Attack))]
    [RequireComponent(typeof(CameraRelativeMovement))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(GravityAndJump))]
    public class Stun : MonoBehaviour
    {
        #region Publics
        public Grabable[] grabables;
        public GameObject physicsRig;
        public GameObject physicsHip;
        [SerializeField] protected PlayerTeleporter playerTeleporter;
        #endregion


        #region Unity Api

        private void Awake()
        {
            _movement = GetComponent<CameraRelativeMovement>();
            _animator = GetComponentInChildren<Animator>();
            _attack = GetComponentInChildren<Attack>();
            _rotation = GetComponent<CameraRelativeRotation>();
            _characterController = GetComponent<CharacterController>();
            _rootRigidBody = GetComponent<Rigidbody>();
       //     _playerInput = GetComponent<PlayerInput>();
            _gravity = GetComponent<GravityAndJump>();
            grabables = physicsRig.GetComponentsInChildren<Grabable>();
        }


        private void OnEnable()
        {
            _movement.enabled = false;
            _attack.enabled = false;
            _rotation.enabled = false;
            _characterController.enabled = false;
           // _playerInput.enabled = false;
            _gravity.enabled = false;
            foreach (Grabable grabable in grabables)
            {
                grabable.enabled = true;
            }
            _animator.SetBool("Stunned", true);
            DisconnectRoot();
        }

        private void OnDisable()
        {
            _movement.enabled = true;
            _attack.enabled = true;
            _rotation.enabled = true;
            _characterController.enabled = true;
        //    _playerInput.enabled = true;
            _gravity.enabled = true;
            foreach (var grabable in grabables)
            {
                grabable.enabled = false;
            }
            _animator.SetBool("Stunned", false);
            TeleportCharacterController();

        }

        private void TeleportCharacterController()
        {
            ConfigurableJointExtended configurableJointExtended = physicsHip.GetComponent<ConfigurableJointExtended>();
            physicsHip.transform.GetPositionAndRotation(out Vector3 position, out Quaternion rotation);
            //teleport player to current position of ragdoll
            _characterController.transform.position = position;
            //Set the position correctly before reconnecting
            physicsHip.transform.SetPositionAndRotation(configurableJointExtended.target.transform.position, configurableJointExtended.target.transform.rotation);
            ReconnectRoot();
            //Reset the position to the start position to maintain the illusion nothing happend
            physicsHip.transform.SetPositionAndRotation(position, rotation);
        }
        #endregion


        #region Main Methods

        #endregion


        #region Utils
        private void DisconnectRoot()
        {
            ConfigurableJoint configurableJoint = physicsHip.GetComponent<ConfigurableJoint>();
            physicsHip.GetComponent<ConfigurableJointExtended>().enabled = false;
            Destroy(configurableJoint);
        }

        private void ReconnectRoot()
        {
            ConfigurableJoint configurableJoint = physicsHip.AddComponent<ConfigurableJoint>();
            ConfigurableJointExtended configurableJointExtended = physicsHip.GetComponent<ConfigurableJointExtended>();
            configurableJointExtended.Reconnect(_rootRigidBody, configurableJoint);
        }
        #endregion


        #region Private and Protected
        private Animator _animator;
        private CameraRelativeMovement _movement;
        private Attack _attack;
        private CameraRelativeRotation _rotation;
        private Rigidbody _rootRigidBody;
        private CharacterController _characterController;
      //  private PlayerInput _playerInput;
        private GravityAndJump _gravity;
        #endregion


    }
}
