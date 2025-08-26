using UnityEngine;
using UnityEngine.InputSystem;

namespace ActiveRagdoll.Runtime
{
    [RequireComponent(typeof(CameraRelativeMovement))]
    public class Attack : MonoBehaviour
    {
        [SerializeField] private Collider attackCollider;
        [SerializeField] private int damage;
        [SerializeField] private float attackDuration = 1;
        private CameraRelativeMovement _movement;
        private Animator _animator;
        public float movementLockDurationLeft = 0;

        private void Awake()
        {
            _movement = GetComponent<CameraRelativeMovement>();
            _animator = GetComponentInChildren<Animator>();
            AttackAnimationEventListener animationEventListener = _animator.gameObject.AddComponent<AttackAnimationEventListener>();
            animationEventListener.Initialize(attackCollider, _animator, _movement);
        }

        public void PlayAttack(InputAction.CallbackContext context)
        {
            if (context.performed && movementLockDurationLeft == 0)
            {
                _animator.SetBool("Attack", true);
                _movement.enabled = false;
                movementLockDurationLeft = attackDuration;
            }
        }

        private void Update()
        {
            if (movementLockDurationLeft > 0)
            {
                movementLockDurationLeft -= Time.deltaTime;
                if (movementLockDurationLeft < 0)
                {
                    movementLockDurationLeft = 0;
                    _animator.SetBool("Attack", false);
                    _movement.enabled = true;
                }
            }
        }
    }
}
