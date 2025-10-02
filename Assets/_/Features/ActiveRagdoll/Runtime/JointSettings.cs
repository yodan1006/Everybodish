using ActiveRagdoll.Runtime;
using UnityEngine;

namespace ActiveRagdoll.Data
{
    [System.Serializable]
    public class JointSettings
    {
        // ConfigurableJoint base properties
        public ConfigurableJointMotion xMotion, yMotion, zMotion;
        public ConfigurableJointMotion angularXMotion, angularYMotion, angularZMotion;

        public SoftJointLimitSpring linearLimitSpring;
        public SoftJointLimit linearLimit;
        public SoftJointLimit lowAngularXLimit, highAngularXLimit, angularYLimit, angularZLimit;

        public JointDrive xDrive, yDrive, zDrive;
        public JointDrive slerpDrive;

        public Vector3 anchor, connectedAnchor;
        public Quaternion targetRotation;
        public Quaternion targetAngularVelocity;

        public Rigidbody connectedBody;

        public float breakForce;
        public float breakTorque;
        public bool enableCollision;
        public bool enablePreprocessing;
        public float projectionDistance;
        public float projectionAngle;
        public bool configuredInWorldSpace;
        public bool swapBodies;

        // Extended Joint Custom Parameters
        public float driveStrengthMultiplier;
        public float driveDampingMultiplier;
        public float maxDriveForceMultiplier;

        public float positionSpring;
        public float positionDamper;
        public float maximumForce;

        public float angularXLimitSpring;
        public float angularXLimitDamper;
        public float angularYZLimitSpring;
        public float angularYZLimitDamper;

        public float linearLimitSpringMultiplier;
        public float linearLimitDamperMultiplier;

        public float boneLength;

        public void ApplyTo(ConfigurableJoint joint, ConfigurableJointExtended extended)
        {
            // Standard joint config
            joint.xMotion = xMotion;
            joint.yMotion = yMotion;
            joint.zMotion = zMotion;

            joint.angularXMotion = angularXMotion;
            joint.angularYMotion = angularYMotion;
            joint.angularZMotion = angularZMotion;

            joint.linearLimitSpring = linearLimitSpring;
            joint.linearLimit = linearLimit;

            joint.lowAngularXLimit = lowAngularXLimit;
            joint.highAngularXLimit = highAngularXLimit;
            joint.angularYLimit = angularYLimit;
            joint.angularZLimit = angularZLimit;

            joint.xDrive = xDrive;
            joint.yDrive = yDrive;
            joint.zDrive = zDrive;
            joint.slerpDrive = slerpDrive;

            joint.anchor = anchor;
            joint.connectedAnchor = connectedAnchor;
            joint.targetRotation = targetRotation;

            joint.connectedBody = connectedBody;

            joint.breakForce = breakForce;
            joint.breakTorque = breakTorque;
            joint.enableCollision = enableCollision;
            joint.enablePreprocessing = enablePreprocessing;
            joint.projectionDistance = projectionDistance;
            joint.projectionAngle = projectionAngle;
            joint.configuredInWorldSpace = configuredInWorldSpace;
            joint.swapBodies = swapBodies;

            // Extended joint config
            if (extended != null)
            {
                extended.driveStrengthMultiplier = driveStrengthMultiplier;
                extended.driveDampingMultiplier = driveDampingMultiplier;
                extended.maxDriveForceMultiplier = maxDriveForceMultiplier;

                extended.positionSpring = positionSpring;
                extended.positionDamper = positionDamper;
                extended.maximumForce = maximumForce;

                extended.angularXLimitSpring = angularXLimitSpring;
                extended.angularXLimitDamper = angularXLimitDamper;
                extended.angularYZLimitSpring = angularYZLimitSpring;
                extended.angularYZLimitDamper = angularYZLimitDamper;

                extended.linearLimitSpring = linearLimitSpring.spring;
                extended.linearLimitDamper = linearLimitSpring.damper;

                extended.projectionDistance = projectionDistance;
                extended.projectionAngle = projectionAngle;

                extended.boneLength = boneLength;
            }
        }

        public static JointSettings FromJoint(ConfigurableJoint joint, ConfigurableJointExtended extended)
        {
            var settings = new JointSettings
            {
                xMotion = joint.xMotion,
                yMotion = joint.yMotion,
                zMotion = joint.zMotion,

                angularXMotion = joint.angularXMotion,
                angularYMotion = joint.angularYMotion,
                angularZMotion = joint.angularZMotion,

                linearLimitSpring = joint.linearLimitSpring,
                linearLimit = joint.linearLimit,

                lowAngularXLimit = joint.lowAngularXLimit,
                highAngularXLimit = joint.highAngularXLimit,
                angularYLimit = joint.angularYLimit,
                angularZLimit = joint.angularZLimit,

                xDrive = joint.xDrive,
                yDrive = joint.yDrive,
                zDrive = joint.zDrive,
                slerpDrive = joint.slerpDrive,

                anchor = joint.anchor,
                connectedAnchor = joint.connectedAnchor,
                targetRotation = joint.targetRotation,

                connectedBody = joint.connectedBody,

                breakForce = joint.breakForce,
                breakTorque = joint.breakTorque,
                enableCollision = joint.enableCollision,
                enablePreprocessing = joint.enablePreprocessing,
                projectionDistance = joint.projectionDistance,
                projectionAngle = joint.projectionAngle,
                configuredInWorldSpace = joint.configuredInWorldSpace,
                swapBodies = joint.swapBodies
            };

            if (extended != null)
            {
                settings.driveStrengthMultiplier = extended.driveStrengthMultiplier;
                settings.driveDampingMultiplier = extended.driveDampingMultiplier;
                settings.maxDriveForceMultiplier = extended.maxDriveForceMultiplier;

                settings.positionSpring = extended.positionSpring;
                settings.positionDamper = extended.positionDamper;
                settings.maximumForce = extended.maximumForce;

                settings.angularXLimitSpring = extended.angularXLimitSpring;
                settings.angularXLimitDamper = extended.angularXLimitDamper;
                settings.angularYZLimitSpring = extended.angularYZLimitSpring;
                settings.angularYZLimitDamper = extended.angularYZLimitDamper;

                settings.linearLimitSpringMultiplier = extended.linearLimitSpring;
                settings.linearLimitDamperMultiplier = extended.linearLimitDamper;

                settings.projectionDistance = extended.projectionDistance;
                settings.projectionAngle = extended.projectionAngle;

                settings.boneLength = extended.boneLength;
            }

            return settings;
        }
    }
}
