using DebugBehaviour.Runtime;
using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class ConfigurableJointExtended : VerboseMonoBehaviour
    {
        public ConfigurableJoint joint;
        private Vector3 initialLocalPosition;
        private Quaternion initialLocalRotation;
        public GameObject target;
        public float boneLength = 0.5f;

        [Header("Joint Drive Settings")]
        public float positionSpring = 15000f;
        public float positionDamper = 400f;
        public float maximumForce = 20000f;

        [Header("Joint Motion Settings")]
        public ConfigurableJointMotion xMotion = ConfigurableJointMotion.Locked;
        public ConfigurableJointMotion yMotion = ConfigurableJointMotion.Locked;
        public ConfigurableJointMotion zMotion = ConfigurableJointMotion.Locked;
        public ConfigurableJointMotion angularXMotion = ConfigurableJointMotion.Limited;
        public ConfigurableJointMotion angularYMotion = ConfigurableJointMotion.Limited;
        public ConfigurableJointMotion angularZMotion = ConfigurableJointMotion.Limited;

        [Header("Limit Spring Settings")]
        public float linearLimitSpring = 4000f;
        public float linearLimitDamper = 300f;
        public float angularXLimitSpring = 5000f;
        public float angularXLimitDamper = 300f;
        public float angularYZLimitSpring = 5000f;
        public float angularYZLimitDamper = 300f;

        [Header("Linear Limit Settings")]
        public float linearBounciness = 0f;
        public float contactDistance = 0.01f;

        [Header("Drive Settings")]
        public RotationDriveMode rotationDriveMode = RotationDriveMode.Slerp;

        [Header("Projection Settings")]
        public JointProjectionMode projectionMode = JointProjectionMode.PositionAndRotation;
        public float projectionDistance = 0.1f;
        public float projectionAngle = 3f;

        [Header("Joint Limits Settings")]
        public float jointBounciness = 0f;
        public float baseLimitRange = 15f;
        public float maxExtraLimitRange = 25f;

        [Header("Global Drive Multipliers")]
        public float driveStrengthMultiplier = 1.0f;
        public float driveDampingMultiplier = 1.0f;
        public float maxDriveForceMultiplier = 1.0f;

        [Header("Debug")]
        public bool drawDebug = true;
        public Color boneColor = Color.green;
        public Color jointAxisColor = Color.cyan;

        public Quaternion InitialLocalRotation { get => initialLocalRotation; }
        public Vector3 InitialLocalPosition { get => initialLocalPosition; }

        // private readonly bool isColliderDisabled = false;

        public void Initialize(GameObject targetObject, Rigidbody connectedBody)
        {
            target = targetObject;
            joint = GetComponent<ConfigurableJoint>();
            ConfigurableJointUtility.SetupAsCharacterJoint(joint);
            joint.connectedBody = connectedBody;
            initialLocalRotation = transform.localRotation;
            initialLocalPosition = transform.localPosition;
            boneLength = GetBoneLength();
            ApplyAdaptiveConfig();
        }

        internal void Reconnect(Rigidbody rootRigidBody, ConfigurableJoint configurableJoint, GameObject targetObject)
        {
            ConfigurableJointUtility.SetupAsCharacterJoint(configurableJoint);
            target = targetObject;
            joint = configurableJoint;
            joint.connectedBody = rootRigidBody;
            boneLength = GetBoneLength();
            ApplyAdaptiveConfig();
            enabled = true;
        }

        private void Awake()
        {
            initialLocalRotation = transform.localRotation;
            initialLocalPosition = transform.localPosition;
            joint = GetComponent<ConfigurableJoint>();
        }

        private void OnDisable()
        {
            //Reset joint rotation to fix Unity's animator bug
            ResetJointRotation();
        }

        public void ResetJointRotation()
        {
            transform.localRotation = initialLocalRotation;
        }

        public void SyncToTargetRotation()
        {
            SetJointRotation(target.transform.rotation);
        }

        public void SetJointRotation(Quaternion newRotation)
        {
            // Apply the inverse of the initial rest pose rotation to remove baked-in tilt
            Quaternion correctedRotation = newRotation * Quaternion.Inverse(initialLocalRotation);

            transform.rotation = correctedRotation;
        }

        private void FixedUpdate()
        {
            if (target != null)
            {
                ConfigurableJointExtensions.SetTargetRotationLocal(joint, target.transform.localRotation, initialLocalRotation);
            }


            float currentBoneLength = GetBoneLength();

            if (currentBoneLength > boneLength * 1.5f)
            {
                //   LogWarning($"{name} overextended (lengh = {boneLength:F3}, distance = {currentBoneLength:F3}), performing hard reset.");

                // Snap position to connectedBody plus initial offset along bone axis
                Vector3 direction = (transform.position - joint.connectedBody.position).normalized;
                Vector3 targetPosition = joint.connectedBody.position + direction * boneLength;
                transform.position = targetPosition;

                // Reset rotation to initial local rotation relative to connected body
                transform.rotation = joint.connectedBody.rotation * initialLocalRotation;

                // Reset Rigidbody velocity and angular velocity to stabilize
                if (TryGetComponent<Rigidbody>(out var rb))
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }

        [ContextMenu("Apply Adaptive Joint Config")]
        public void ApplyAdaptiveConfigInEditor()
        {
            joint = GetComponent<ConfigurableJoint>();
            if (joint == null)
            {
                Debug.LogWarning("No ConfigurableJoint found.");
                return;
            }

            boneLength = GetBoneLength();
            ApplyAdaptiveConfig();
        }

        private void ApplyAdaptiveConfig()
        {
            float normalized = Mathf.InverseLerp(0.01f, 1f, boneLength);

            // Fixed: Always use Locked or Limited motion for better stability
            joint.xMotion = xMotion;
            joint.yMotion = yMotion;
            joint.zMotion = zMotion;
            joint.angularXMotion = angularXMotion;
            joint.angularYMotion = angularYMotion;
            joint.angularZMotion = angularZMotion;

            // Spring settings
            float springScale = Mathf.Lerp(1.0f, 1.4f, normalized);
            joint.linearLimitSpring = new SoftJointLimitSpring
            {
                spring = linearLimitSpring * springScale,
                damper = linearLimitDamper * springScale
            };
            joint.angularXLimitSpring = new SoftJointLimitSpring
            {
                spring = angularXLimitSpring * springScale,
                damper = angularXLimitDamper * springScale
            };
            joint.angularYZLimitSpring = new SoftJointLimitSpring
            {
                spring = angularYZLimitSpring * springScale,
                damper = angularYZLimitDamper * springScale
            };

            // Linear limit
            joint.linearLimit = new SoftJointLimit
            {
                limit = Mathf.Lerp(0.03f, 0.15f, normalized),
                bounciness = linearBounciness,
                contactDistance = contactDistance
            };

            // Stronger drive forces
            JointDrive drive = new()
            {
                positionSpring = positionSpring * Mathf.Lerp(2f, 5f, normalized) * driveStrengthMultiplier,
                positionDamper = positionDamper * Mathf.Lerp(1f, 3f, normalized) * driveDampingMultiplier,
                maximumForce = maximumForce * Mathf.Lerp(5f, 10f, normalized) * maxDriveForceMultiplier
            };

            joint.rotationDriveMode = rotationDriveMode;
            if (rotationDriveMode == RotationDriveMode.Slerp)
            {
                joint.slerpDrive = drive;
            }
            else
            {
                joint.angularXDrive = drive;
                joint.angularYZDrive = drive;
            }

            // Adaptive Joint Limits
            Vector3 angleDiff = ConfigurableJointUtility.GetRotationDifference(joint, joint.connectedBody, transform);
            ConfigurableJointUtility.ApplyAdaptiveLimits(joint, angleDiff, boneLength, jointBounciness, baseLimitRange, maxExtraLimitRange);

            // Projection
            joint.projectionMode = projectionMode;
            joint.projectionDistance = projectionDistance;
            joint.projectionAngle = projectionAngle;
        }

        private float GetBoneLength()
        {
            if (joint.connectedBody == null)
            {
                Debug.LogWarning("No connected body found for bone length calculation.", this);
                return 0.5f;
            }
            return Vector3.Distance(transform.position, joint.connectedBody.position);
        }

        private void OnDrawGizmos()
        {
            if (joint != null && joint.connectedBody != null)
            {
                float dist = Vector3.Distance(transform.position, joint.connectedBody.position);
                if (dist > boneLength * 1.5f)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(transform.position, joint.connectedBody.position);
                }
            }
            Gizmos.color = boneColor;
            if (joint != null && joint.connectedBody != null)
                Gizmos.DrawLine(transform.position, joint.connectedBody.position);

            Gizmos.color = jointAxisColor;
            Gizmos.DrawRay(transform.position, transform.forward * 0.2f);
        }


        public static class ConfigurableJointUtility
        {
            public static void SetupAsCharacterJoint(ConfigurableJoint joint)
            {
                joint.rotationDriveMode = RotationDriveMode.Slerp;
                joint.xMotion = ConfigurableJointMotion.Locked;
                joint.yMotion = ConfigurableJointMotion.Locked;
                joint.zMotion = ConfigurableJointMotion.Locked;
                joint.angularXMotion = ConfigurableJointMotion.Limited;
                joint.angularYMotion = ConfigurableJointMotion.Limited;
                joint.angularZMotion = ConfigurableJointMotion.Limited;
            }

            public static Vector3 GetRotationDifference(ConfigurableJoint joint, Rigidbody connectedBody, Transform target)
            {
                Quaternion restRelativeRotation = Quaternion.Inverse(connectedBody.rotation) * target.rotation;
                Quaternion restWorldRotation = connectedBody.rotation * restRelativeRotation;
                Quaternion deltaRotation = Quaternion.Inverse(restWorldRotation) * target.rotation;
                return NormalizeEuler(deltaRotation.eulerAngles);
            }

            public static void ApplyAdaptiveLimits(ConfigurableJoint joint, Vector3 angleDiff, float boneLength, float bounciness, float baseRange, float extraRange)
            {
                float normalized = Mathf.InverseLerp(0.01f, 0.5f, boneLength);
                float margin = baseRange + (extraRange * normalized);

                float maxX = Mathf.Clamp(Mathf.Abs(angleDiff.x) + margin, baseRange, baseRange + extraRange);
                float maxY = Mathf.Clamp(Mathf.Abs(angleDiff.y) + margin, baseRange, baseRange + extraRange);
                float maxZ = Mathf.Clamp(Mathf.Abs(angleDiff.z) + margin, baseRange, baseRange + extraRange);

                joint.lowAngularXLimit = new SoftJointLimit { limit = -maxX, bounciness = bounciness };
                joint.highAngularXLimit = new SoftJointLimit { limit = maxX, bounciness = bounciness };
                joint.angularYLimit = new SoftJointLimit { limit = maxY, bounciness = bounciness };
                joint.angularZLimit = new SoftJointLimit { limit = maxZ, bounciness = bounciness };
            }

            private static Vector3 NormalizeEuler(Vector3 angles)
            {
                return new Vector3(
                    Mathf.DeltaAngle(0, angles.x),
                    Mathf.DeltaAngle(0, angles.y),
                    Mathf.DeltaAngle(0, angles.z)
                );
            }
        }
    }
}
