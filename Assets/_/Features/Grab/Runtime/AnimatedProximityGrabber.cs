using Grab.Data;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Grab.Runtime
{
    public class AnimatedProximityGrabber : ProximityGrabber, IAnimatedProximityGrabber
    {
        private Animator animator;
        private int grabLayerIndex;
        // Grab Layer
        private new void Awake()
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

        public new bool Release()
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

        public new void OnRelease(CallbackContext callbackContext)
        {
            Release();
        }

        public new void OnGrabAction(CallbackContext callbackContext)
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

        public void OnHoldGrabAction(CallbackContext callbackContext)
        {
            if (callbackContext.performed)
            {
                OnGrabAction(callbackContext);
            }
            else if (callbackContext.canceled)
            {
                OnRelease(callbackContext);
            }
        }

        public new bool TryGrab(IGrabable newGrabable)
        {
            return base.TryGrab(newGrabable);
        }
    }
}
