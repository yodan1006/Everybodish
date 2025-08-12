using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ConfigurableJoint))]
    public class ConfigurableJointExtended : MonoBehaviour
    {
        public ConfigurableJoint joint;
        public Quaternion initialLocalRotation;
        public GameObject target;
        public Transform parentTransform;

        [Header("Joint Drive Settings")]
        public float positionSpring = 1000f;
        public float positionDamper = 10f;
        public float maximumForce = 50f;

        [Header("Joint Limits Settings")]
        public float jointBounciness = 0f;
        public float baseLimitRange = 20f;
        public float maxExtraLimitRange = 70f;

        [Header("Runtime Settings")]
        public bool autoUpdateDrive = false;
        public bool autoUpdateLimits = false;

        [Header("Debug")]
        public bool drawDebug = true;
        public Color boneColor = Color.green;
        public Color jointAxisColor = Color.cyan;

        public void Initialize(GameObject targetObject, Rigidbody connectedBody)
        {
            target = targetObject;
            parentTransform = transform.parent;
            initialLocalRotation = transform.localRotation;
            joint = GetComponent<ConfigurableJoint>();
            ConfigurableJointUtility.SetupAsCharacterJoint(joint);
            joint.connectedBody = connectedBody;
            AutoConfigureJointDrive();
            AutoConfigureJointLimits();
        }

        private void FixedUpdate()
        {
            if (target != null)
            {
                ConfigurableJointExtensions.SetTargetRotationLocal(joint, target.transform.localRotation, initialLocalRotation);
            }

            if (autoUpdateDrive)
                AutoConfigureJointDrive();

            if (autoUpdateLimits)
                AutoConfigureJointLimits();
        }

        private void AutoConfigureJointDrive()
        {
            float boneLength = GetBoneLength();
            ConfigurableJointUtility.ApplyScaledDrive(joint, boneLength, positionSpring, positionDamper, maximumForce);
        }

        private void AutoConfigureJointLimits()
        {
            float boneLength = GetBoneLength();
            Vector3 angleDiff = ConfigurableJointUtility.GetRotationDifference(joint, joint.connectedBody, transform);
            ConfigurableJointUtility.ApplyAdaptiveLimits(joint, angleDiff, boneLength, jointBounciness, baseLimitRange, maxExtraLimitRange);
        }

        private float GetBoneLength()
        {
            if (parentTransform == null)
                parentTransform = transform.parent;

            return parentTransform != null ? Vector3.Distance(transform.position, parentTransform.position) : 0.1f;
        }

        private void OnDrawGizmos()
        {
            if (!drawDebug) return;

            Gizmos.color = boneColor;
            if (transform.parent != null)
                Gizmos.DrawLine(transform.position, transform.parent.position);

            Gizmos.color = jointAxisColor;
            Vector3 jointAxis = transform.forward * 0.2f;
            Gizmos.DrawRay(transform.position, jointAxis);
        }

        public static class ConfigurableJointUtility
        {
            public static void SetupAsCharacterJoint(ConfigurableJoint joint)
            {
                joint.rotationDriveMode = RotationDriveMode.Slerp;
                joint.angularXMotion = ConfigurableJointMotion.Limited;
                joint.angularYMotion = ConfigurableJointMotion.Limited;
                joint.angularZMotion = ConfigurableJointMotion.Limited;
                joint.xMotion = ConfigurableJointMotion.Locked;
                joint.yMotion = ConfigurableJointMotion.Locked;
                joint.zMotion = ConfigurableJointMotion.Locked;
            }

            public static void ApplyScaledDrive(ConfigurableJoint joint, float boneLength, float spring, float damper, float force)
            {
                float normalized = Mathf.InverseLerp(0.01f, 1f, boneLength);

                float springScale = Mathf.Lerp(2f, 0.3f, normalized);
                float damperScale = Mathf.Lerp(2f, 0.3f, normalized);
                float forceScale = Mathf.Lerp(1f, 2f, normalized);

                JointDrive drive = new()
                {
                    positionSpring = spring * springScale,
                    positionDamper = damper * damperScale,
                    maximumForce = force * forceScale
                };

                joint.slerpDrive = drive;
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