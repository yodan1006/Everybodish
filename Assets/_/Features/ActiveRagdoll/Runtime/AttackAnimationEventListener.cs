using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class AttackAnimationEventListener : MonoBehaviour
    {
        #region Publics
        public void Initialize(Collider attackCollider, Animator animator, CameraRelativeMovement movement)
        {
            this.attackCollider = attackCollider;
            this.animator = animator;
            this.movement = movement;
        }
        #endregion


        #region Unity Api

        #endregion


        #region Main Methods
        public void AnimEventActiveHeadButt()
        {
            attackCollider.enabled = true;
            Debug.Log("AnimEventActiveHeadButt");
        }

        public void AnimEventDisableHeadButt()
        {
            Debug.Log("AnimEventDisableHeadButt");
            attackCollider.enabled = false;
            animator.SetBool("Attack", false);
        }
        #endregion


        #region Utils

        #endregion


        #region Private and Protected
        private Collider attackCollider;
        private Animator animator;
        private CameraRelativeMovement movement;
        #endregion


    }
}
