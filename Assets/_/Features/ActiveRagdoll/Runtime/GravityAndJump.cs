using UnityEngine;
using UnityEngine.InputSystem;

namespace ActiveRagdoll.Runtime
{
    public class GravityAndJump : MonoBehaviour
    {
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


            // Apply movement and gravity
            Vector3 velocity = new();
            velocity.y = verticalVelocity;

            controller.Move(velocity * Time.deltaTime);
        }
    }
}
