using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace PlayerLocomotion.Runtime
{
    public class CameraRelativeRotation : MonoBehaviour
    {
        public bool inverted;
        [Header("Movement Settings")]
        public float rotationSpeed = 8f;
        public Transform cameraTransform;


        private Vector2 inputMovement;

        private void Awake()
        {
            if (cameraTransform == null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        //brandon modif 

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (Camera.main != null)
                cameraTransform = Camera.main.transform;
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            //brandon
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        // -----------------------------------------------------------------

        public void OnMovement(InputAction.CallbackContext context)
        {
            inputMovement = context.ReadValue<Vector2>();
        }
        private void Update()
        {
            // modif brandon perte de camera

            if (cameraTransform == null) return;

            //------------------------------------

            if (transform.gameObject != cameraTransform.gameObject)
            {
                // Camera-relative rotation
                Vector3 camForward = GetGroundedVector(cameraTransform.forward);
                Vector3 camRight = GetGroundedVector(cameraTransform.right);
                Vector3 moveDir = camForward * inputMovement.y + camRight * inputMovement.x;
                moveDir = moveDir.normalized;
                if (inverted == true)
                {
                    moveDir *= -1;
                }
                // Rotate character towards movement direction
                if (moveDir.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDir);

                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }


        private static Vector3 GetGroundedVector(Vector3 vector)
        {
            vector.y = 0f;
            return vector.normalized;
        }
    }
}
