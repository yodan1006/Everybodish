using System.Collections;
using System.Security.Cryptography.X509Certificates;
using MovePlayer.Runtime;
using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    [DisallowMultipleComponent]
    public class AttackAnimationEventListener : MonoBehaviour
    {
        #region Publics
        public void Initialize(Attack attack, Animator animator, AttackTrigger attackTrigger)
        {
            this.attack = attack;
            this.attackTrigger = attackTrigger;
            this.animator = animator;
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
        #endregion


    }
}
