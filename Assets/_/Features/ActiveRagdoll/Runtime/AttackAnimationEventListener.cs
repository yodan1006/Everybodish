using MovePlayer.Runtime;
using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class AttackAnimationEventListener : MonoBehaviour
    {
        #region Publics
        public void Initialize(AttackTrigger attackTrigger, Animator animator, CameraRelativeMovement movement)
        {
            this.attackTrigger = attackTrigger;
            this.animator = animator;
            this.movement = movement;
        }
        #endregion


        #region Unity Api

        #endregion


        #region Main Methods
        public void AnimEventActiveHeadButt()
        {
            attackTrigger.enabled = true;
            Debug.Log("AnimEventActiveHeadButt");
        }

        public void AnimEventDisableHeadButt()
        {
            Debug.Log("AnimEventDisableHeadButt");
            attackTrigger.enabled = false;
        }
        #endregion


        #region Utils

        #endregion


        #region Private and Protected
        private AttackTrigger attackTrigger;
        private Animator animator;
        private CameraRelativeMovement movement;
        #endregion


    }
}
