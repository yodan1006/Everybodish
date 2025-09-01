using Grab.Runtime;
using MovePlayer.Runtime;
using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    [DisallowMultipleComponent]
    public class AttackAnimationEventListener : MonoBehaviour
    {
        #region Publics
        public float throwObjectForce = 7;
        public void Initialize(Attack attack, Animator animator, AttackTrigger attackTrigger, AnimatedProximityGrabber proximityGrabber)
        {
            this.attack = attack;
            this.attackTrigger = attackTrigger;
            this.animator = animator;
            this.proximityGrabber = proximityGrabber;
        }
        #endregion


        #region Unity Api

        #endregion


        #region Main Methods
        public void AnimEventActiveHeadButt()
        {
            if (!proximityGrabber.IsGrabbing())
            {
            attackTrigger.enabled = true;
            Debug.Log("AnimEventActiveHeadButt");
            }else
            {
                Grab.Data.IGrabable grabable = proximityGrabber.Grabable;
                proximityGrabber.Release();
                if(grabable.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    rb.linearVelocity =(transform.forward + transform.up).normalized * throwObjectForce;
                }
            }

        }

        public void AnimEventDisableHeadButt()
        {
            Debug.Log("AnimEventDisableHeadButt");
            attackTrigger.enabled = false;
        }

        public void AttackStart()
        {
            attack.SetSpeedMultiplier(true);
        }

        public void AttackEnd()
        {
            attack.SetSpeedMultiplier(false);
        }


        #endregion


        #region Utils

        #endregion


        #region Private and Protected
        private AttackTrigger attackTrigger;
        private Attack attack;
        private Animator animator;
        private ProximityGrabber proximityGrabber;
        #endregion


    }
}
