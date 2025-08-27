using MovePlayer.Runtime;
using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    [DisallowMultipleComponent]
    public class AttackAnimationEventListener : MonoBehaviour
    {
        #region Publics
        public void Initialize(AttackTrigger attackTrigger)
        {
            this.attackTrigger = attackTrigger;
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
        #endregion


    }
}
