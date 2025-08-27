using ActiveRagdoll.Runtime;
using MovePlayer.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CameraRelativeMovement))]
public class Attack : MonoBehaviour
{
    [SerializeField] private Collider attackCollider;
    [SerializeField] private int damage;
    [SerializeField] private float attackDuration = 1;
    private CameraRelativeMovement _movement;
    private Animator _animator;
    private AttackTrigger _attackTrigger;
    public float movementLockDurationLeft = 0;
    public float attackMoveSpeedMultiplier = 0.5f;

    private void Awake()
    {
        _movement = GetComponent<CameraRelativeMovement>();
        _animator = GetComponentInChildren<Animator>();
        _attackTrigger = GetComponentInChildren<AttackTrigger>();

        AttackAnimationEventListener animationEventListener = _animator.gameObject.AddComponent<AttackAnimationEventListener>();
        animationEventListener.Initialize(_attackTrigger);
    }

    public void PlayAttack(InputAction.CallbackContext context)
    {
        if (context.performed && movementLockDurationLeft == 0)
        {
            _animator.SetBool("Attack", true);
            _movement.moveSpeedMultiplier = attackMoveSpeedMultiplier;
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
                _movement.moveSpeedMultiplier = 1f;
            }
        }
    }
}