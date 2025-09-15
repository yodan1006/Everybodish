using System.Collections.Generic;
using Grab.Data;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Grab.Runtime
{
    public class AnimatedProximityGrabber : ProximityGrabber, IAnimatedProximityGrabber
    {
        private Animator animator;
        private int grabLayerIndex;
        // Grab Layer
        protected new void Awake()
        {
            base.Awake();
            bool found = false;
            animator = GetComponentInChildren<Animator>();
            for (int i = 0; i < animator.layerCount; i++)
            {
                if (animator.GetLayerName(i).ToLower().Contains("grab"))
                {
                    grabLayerIndex = i;
                    found = true;
                }
            }
            if (found == false)
            {
                LogWarning("Grab Layer not found!", this);
            }
        }

        private void OnDisable()
        {
            Release();
        }

        public override bool Release()
        {
            bool success = false;
            if (IsGrabbing())
            {
                if (base.Release())
                {
                    animator.SetLayerWeight(grabLayerIndex, 0);
                    success = true;
                }
            }
            return success;
        }

        public override void OnRelease(CallbackContext callbackContext)
        {
            if (enabled)
            {
                Release();
            }
        }

        public override void OnGrabAction(CallbackContext callbackContext)
        {
            if (enabled)
            {
                TryGrab();
            }
        }

        public void TryGrab()
        {
            if (!IsGrabbing())
            {
                Log("Grab");
                Collider[] colliders = GetCollidersInArea();
                Log($"Found {colliders.Length} colliders", this);
                List<IGrabable> grabables = GetGrabables(colliders);
                Log($"Found {grabables.Count} grabables", this);
                if (TryGrabClosestAvailable(grabables))
                {
                    animator.SetLayerWeight(grabLayerIndex, 1);
                }
            }
        }

        public void TryGrabReleaseAction(CallbackContext callbackContext)
        {
            if (enabled)
            {
                if (callbackContext.performed)
                {
                    if (IsGrabbing())
                    {
                        Release();

                    }
                    else
                    {
                        TryGrab();
                    }
                }
            }
        }

        public void OnHoldGrabAction(CallbackContext callbackContext)
        {
            if (enabled)
            {
                if (callbackContext.performed)
                {
                    TryGrab();
                }
                else if (callbackContext.canceled)
                {
                    Release();
                }
            }
        }

        public override bool TryGrab(IGrabable newGrabable)
        {
            bool success = false;
            if (base.TryGrab(newGrabable))
            {
                animator.SetLayerWeight(grabLayerIndex, 1);
                success = true;
            }
            return success; ;
        }
    }
}
