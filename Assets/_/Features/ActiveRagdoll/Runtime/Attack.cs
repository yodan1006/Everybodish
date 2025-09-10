using ActiveRagdoll.Runtime;
using Grab.Runtime;
using MovePlayer.Runtime;
using PlayerLocomotion.Runtime;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CameraRelativeMovement))]
[RequireComponent(typeof(AnimatedProximityGrabber))]
public class Attack : MonoBehaviour
{
    [SerializeField] private Collider attackCollider;
    [SerializeField] private int damage;
    private CameraRelativeMovement _movement;
    private Animator _animator;
    private AttackTrigger _attackTrigger;
    private AnimatedProximityGrabber _proximityGrabber;
    public float attackMoveSpeedMultiplier = 0.5f;
    public float throwObjectForce = 7f;

    private void Awake()
    {
        _movement = GetComponent<CameraRelativeMovement>();
        _animator = GetComponentInChildren<Animator>();
        _attackTrigger = GetComponentInChildren<AttackTrigger>();
        _proximityGrabber = GetComponent<AnimatedProximityGrabber>();
        if (!_animator.gameObject.TryGetComponent<AttackAnimationEventListener>(out AttackAnimationEventListener listener))
        {
            AttackAnimationEventListener animationEventListener = _animator.gameObject.AddComponent<AttackAnimationEventListener>();
            animationEventListener.Initialize(this, _animator, _attackTrigger, _proximityGrabber, throwObjectForce);
        }
        else
        {
            listener.Initialize(this, _animator, _attackTrigger, _proximityGrabber, throwObjectForce);
        }

    }

    public void PlayAttack(CallbackContext context)
    {
        if (context.performed)
        {
            _animator.SetTrigger("Attack");
        }
    }

    public void SetSpeedMultiplier(bool isSet)
    {
        if (isSet)
        {
            _movement.moveSpeedMultiplier = attackMoveSpeedMultiplier;
        }
        else
        {
            _movement.moveSpeedMultiplier = 1;
        }
    }
}