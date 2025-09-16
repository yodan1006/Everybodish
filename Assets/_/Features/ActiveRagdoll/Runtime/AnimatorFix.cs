using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    // To fix the animator: check write defaults on all animations
    // Add this to the animator
    // Manually reset the bones on disable
    // #UnityFixYourShit
    public class AnimatorFix : MonoBehaviour
    {
        private Animator animator;

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
