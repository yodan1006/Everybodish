using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace PlayerLocomotion.Runtime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public class CameraRelativeMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        public bool inverted = false;
        public float baseMoveSpeed = 5f;
        public float moveSpeedMultiplier = 1f;
        public Transform cameraTransform;


        private CharacterController controller;
        private Animator animator;

        private Vector2 inputMovement;
        private readonly float verticalVelocity;
        private bool isGrounded;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            if (cameraTransform == null) cameraTransform = Camera.main.transform;
            controller.enabled = true;
        }

        // BRANDON modif

        private void Start()
        {
            if (Camera.main != null) cameraTransform = Camera.main.transform;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //BRANDON modif
        //--------------------------------------------------------------
        private void OnDisable()
        {
            SetAnimatorMoveSpeed(0);

            //brandon
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            inputMovement = context.ReadValue<Vector2>();
        }


        private void Update()
        {
            // modif brandon perte de camera

            if (cameraTransform == null) return;


            // Ground check
            isGrounded = controller.isGrounded;

            // Camera-relative movement
            Vector3 camForward = GetGroundedVector(cameraTransform.forward);
            Vector3 camRight = GetGroundedVector(cameraTransform.right);
            Vector3 moveDir = camForward * inputMovement.y + camRight * inputMovement.x;
            moveDir = moveDir.normalized;

            // Apply movement and gravity
            Vector3 velocity = moveDir * baseMoveSpeed * inputMovement.magnitude * moveSpeedMultiplier;
            velocity.y = verticalVelocity;

            controller.Move(velocity * Time.deltaTime);
            if (inverted == false)
            {
                SetAnimatorMoveSpeed(inputMovement.magnitude);

            }
            else
            {
                //Play animation backwards, needs changes in animator
                //animator.SetFloat("speedMove", -inputMovement.magnitude);
                SetAnimatorMoveSpeed(inputMovement.magnitude);
            }
        }

        private void SetAnimatorMoveSpeed(float speed)
        {
            animator.SetFloat("MoveSpeed", inputMovement.magnitude);
        }

        private static Vector3 GetGroundedVector(Vector3 vector)
        {
            vector.y = 0f;
            return vector.normalized;
        }
    }
}