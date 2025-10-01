using ActiveRagdoll.Runtime;
using UnityEngine;

namespace ActiveRagdoll.Data
{
    [System.Serializable]
    public class JointSettings
    {
        public ConfigurableJointMotion xMotion, yMotion, zMotion;
        public ConfigurableJointMotion angularXMotion, angularYMotion, angularZMotion;
        public SoftJointLimitSpring linearLimitSpring;
        public SoftJointLimit linearLimit;
        public JointDrive xDrive, yDrive, zDrive;
        public JointDrive slerpDrive;
        public Vector3 anchor, connectedAnchor;
        public Quaternion targetRotation;
        public Rigidbody connectedBody;

        public void ApplyTo(ConfigurableJoint joint, ConfigurableJointExtended extendedJoint)
        {
            joint.xMotion = xMotion;
            joint.yMotion = yMotion;
            joint.zMotion = zMotion;

            joint.angularXMotion = angularXMotion;
            joint.angularYMotion = angularYMotion;
            joint.angularZMotion = angularZMotion;

            joint.linearLimitSpring = linearLimitSpring;
            joint.linearLimit = linearLimit;

            joint.xDrive = xDrive;
            joint.yDrive = yDrive;
            joint.zDrive = zDrive;
            joint.slerpDrive = slerpDrive;

            joint.anchor = anchor;
            joint.connectedAnchor = connectedAnchor;
            joint.targetRotation = targetRotation;
            joint.connectedBody = connectedBody;
            //extendedJoint.Initialize
        }

        public static JointSettings FromJoint(ConfigurableJoint joint, ConfigurableJointExtended extendedJoint)
        {
            return new JointSettings
            {
                xMotion = joint.xMotion,
                yMotion = joint.yMotion,
                zMotion = joint.zMotion,

                angularXMotion = joint.angularXMotion,
                angularYMotion = joint.angularYMotion,
                angularZMotion = joint.angularZMotion,

                linearLimitSpring = joint.linearLimitSpring,
                linearLimit = joint.linearLimit,

                xDrive = joint.xDrive,
                yDrive = joint.yDrive,
                zDrive = joint.zDrive,
                slerpDrive = joint.slerpDrive,

                anchor = joint.anchor,
                connectedAnchor = joint.connectedAnchor,
                targetRotation = joint.targetRotation,

                connectedBody = joint.connectedBody
            };
        }
    }

}
