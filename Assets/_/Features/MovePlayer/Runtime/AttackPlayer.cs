using UnityEngine;
using UnityEngine.InputSystem;

namespace MovePlayer.Runtime
{
    [RequireComponent(typeof(PlayerStat))]
    [RequireComponent(typeof(Collider))]
    public class AttackPlayer : MonoBehaviour
    {
        [SerializeField] private Collider colliderToAttack;
        [SerializeField] private int damage;
        [SerializeField] private Animator animator;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            other.GetComponent<PlayerStat>().HurtPlayer(1);
        }

        public void Attack(InputAction.CallbackContext context)
        {
            animator.SetBool("Attack", true);
        }

        public void AnimEventActiveHeadButt()
        {
            colliderToAttack.enabled = true;
        }

        public void AnimEventDisableHeadButt()
        {
            colliderToAttack.enabled = false;
        }
    }
}
