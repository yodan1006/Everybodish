using UnityEngine;
using UnityEngine.InputSystem;

namespace MovePlayer.Runtime
{
    [RequireComponent(typeof(Rigidbody))]
    public class Move : MonoBehaviour
    {
        private Vector2 _move;
        private Rigidbody _rb;

        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Collider zoneAttack;

        [SerializeField] private float _timeResetAttack;
        [SerializeField] private Animator animator;


        private void Awake()
        {
            zoneAttack.enabled = false;
            _rb = GetComponent<Rigidbody>();
            PlayerInput playerInput = GetComponent<PlayerInput>();
            foreach (var map in playerInput.actions.actionMaps)
            {
                if (map.name != "Player")
                    map.Disable();
            }
            playerInput.actions.FindActionMap("Player").Enable();
        }
        

        private void Update()
        {
            Vector3 move = new(_move.x, 0f, _move.y);
            Vector3 input = new(_move.x, 0f, _move.y);

            if (input.sqrMagnitude > 0.0001f)
            {
                Vector3 dir = input.normalized;
                Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
                Quaternion newRot = Quaternion.Slerp(_rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
                _rb.MoveRotation(newRot);
            }


            _rb.MovePosition(transform.position + speed * Time.deltaTime * move);
            
            }
        

        public void MovePlayer(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
            animator.SetFloat("speedMove", _move.magnitude);
        }

        public void Attack(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                AttackEnable();
                animator.SetBool("Attack", true);
            }
        }

        public void AttackEnable()
        {
            zoneAttack.enabled = true;
        }

        public void AttackDisable()
        {
            zoneAttack.enabled = false;
            animator.SetBool("Attack", false);
        }
    }
}
