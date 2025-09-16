using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    // #Fix your shit Unity
    public class AnimatorFix : MonoBehaviour
    {
        Animator animator;

        private void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
                animator.keepAnimatorStateOnDisable = true;
            }
        }
    }
}
