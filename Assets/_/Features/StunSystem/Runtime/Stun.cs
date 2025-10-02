using ActiveRagdoll.Runtime;
using Grab.Runtime;
using Machine.Runtime;
using PlayerLocomotion.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace StunSystem.Runtime
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Attack))]
    [RequireComponent(typeof(CameraRelativeMovement))]
    [RequireComponent(typeof(GravityAndJump))]

    public class Stun : MonoBehaviour
    {
        #region Publics
        public Grabable[] grabables;
        public GameObject physicsRig;
        public GameObject physicsHip;
        [SerializeField] protected PlayerTeleporter playerTeleporter;
        public ParticleSystem stunEffect;
        public UnityEvent<bool> onEnableActions = new();
        #endregion


        #region Unity Api

        private void Awake()
        {
            _movement = GetComponent<CameraRelativeMovement>();
            _animator = GetComponentInChildren<Animator>();
            _attack = GetComponentInChildren<Attack>();
            _rotation = GetComponent<CameraRelativeRotation>();
            _characterController = GetComponent<CharacterController>();
            _gravity = GetComponent<GravityAndJump>();
            _animatedProximityGrabber = GetComponent<AnimatedProximityGrabber>();
            grabables = physicsRig.GetComponentsInChildren<Grabable>();
        }


        private void OnEnable()
        {
            _movement.enabled = false;
            _attack.enabled = false;
            _rotation.enabled = false;
            _characterController.enabled = false;
            _gravity.enabled = false;
            _animatedProximityGrabber.enabled = false;
            foreach (Grabable grabable in grabables)
            {
                grabable.enabled = true;
            }
            _animator.SetBool("Stunned", true);
            playerTeleporter.DisconnectRoot();
            stunEffect.Play();
            onEnableActions.Invoke(false);
        }

        private void OnDisable()
        {
            _movement.enabled = true;
            _attack.enabled = true;
            _rotation.enabled = true;
            _characterController.enabled = true;
            _gravity.enabled = true;
            _animatedProximityGrabber.enabled = true;
            foreach (var grabable in grabables)
            {
                grabable.enabled = false;
            }
            _animator.SetBool("Stunned", false);
            playerTeleporter.ReconnectCharacterControllerToRagdoll();
            stunEffect.Stop();
            onEnableActions.Invoke(true);
        }

        private void Update()
        {
            stunDuration -= Time.deltaTime;
            if (stunDuration < 0)
            {
                enabled = false;
            }
        }

        #endregion


        #region Main Methods
        public void StunForDuration(float stunDuration)
        {
            enabled = true;
            this.stunDuration = stunDuration;
        }
        #endregion


        #region Utils


        #endregion


        #region Private and Protected
        private Animator _animator;
        private CameraRelativeMovement _movement;
        private Attack _attack;
        private CameraRelativeRotation _rotation;
        private CharacterController _characterController;
        private GravityAndJump _gravity;
        private AnimatedProximityGrabber _animatedProximityGrabber;
        private float stunDuration;
        #endregion


    }
}
