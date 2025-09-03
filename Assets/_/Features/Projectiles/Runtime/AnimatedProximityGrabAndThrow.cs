using ActiveRagdoll.Runtime;
using Grab.Runtime;

namespace Projectiles.Runtime
{
    public class AnimatedProximityGrabAndThrow : AnimatedProximityGrabber
    {
        public float minimumThrowVelocity = 1f;
        public override bool Release()
        {
            bool release = false;
            if (base.Release())
            {
                if (heldRigidbody.linearVelocity.magnitude > damageVelocityThreshold)
                {
                    heldRigidbody.gameObject.AddComponent<ProjectileHurtCollider>();
                }
                release = true;
            }
            return release;
        }

    }
}
