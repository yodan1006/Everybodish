using UnityEngine;
using UnityEngine.InputSystem;

namespace MovePlayer.Runtime
{
    [RequireComponent(typeof(Rigidbody))]
    public class Move : MonoBehaviour
    {
        private Vector2 _move;
        private Rigidbody _rb;

        [SerializeField] private float speedWalked;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Animator animator;


        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            // PlayerInput playerInput = GetComponent<PlayerInput>();
            // foreach (var map in playerInput.actions.actionMaps)
            // {
            //     if (map.name != "Player")
            //         map.Disable();
            // }
            // playerInput.actions.FindActionMap("Player").Enable();
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

            _rb.MovePosition(transform.position + speedWalked * Time.deltaTime * move);

        }


        public void MovePlayer(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
            animator.SetFloat("speedMove", _move.magnitude);
        }
    }
}
