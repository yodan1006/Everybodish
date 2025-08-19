using UnityEngine;
using UnityEngine.InputSystem;

namespace ActiveRagdoll.Runtime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public class CameraRelativeMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float rotationSpeed = 10f;
        public Transform cameraTransform;

        [Header("Gravity Settings")]
        public float gravity = -9.81f;
        public float jumpHeight = 2f;

        private CharacterController controller;
        private Animator animator;

        private Vector2 inputMovement;
        private float verticalVelocity;
        private bool isGrounded;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
        }

        private void OnDisable()
        {
            animator.SetFloat("Move", 0);
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            inputMovement = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && isGrounded)
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }



        private void Update()
        {
            // Ground check
            isGrounded = controller.isGrounded;

            if (isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = -2f; // Keeps the character grounded
            }
            else
            {
                verticalVelocity += gravity * Time.deltaTime;
            }

            // Camera-relative movement
            Vector3 camForward = GetGroundedVector(cameraTransform.forward);
            Vector3 camRight = GetGroundedVector(cameraTransform.right);
            Vector3 moveDir = camForward * inputMovement.y + camRight * inputMovement.x;
            moveDir = moveDir.normalized;

            // Apply movement and gravity
            Vector3 velocity = moveDir * moveSpeed * inputMovement.magnitude;
            velocity.y = verticalVelocity;

            controller.Move(velocity * Time.deltaTime);

            // Rotate character towards movement direction
            if (moveDir.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            // Animation
            animator.SetFloat("Move", inputMovement.magnitude);
        }

        private static Vector3 GetGroundedVector(Vector3 vector)
        {
            vector.y = 0f;
            return vector.normalized;
        }
    }
}