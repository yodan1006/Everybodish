using Interactions.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Interactions.Runtime
{
    public class TimedInteractor : CooldownInteractor, ITimedInteractor
    {
        [Header("Interaction Settings")]
        [Tooltip("How long the interaction should last in seconds.")]
        [SerializeField] private float interactionDuration = 3f;

        private float interactionTimer;
        private bool isInteracting;

        [Header("Events")]
        public UnityEvent onInteractionStart = new();
        public UnityEvent onInteractionEnd = new();
        public UnityEvent onInteractionCanceled = new();

        public UnityEvent OnInteractionStart => onInteractionStart;
        public UnityEvent OnInteractionEnd => onInteractionEnd;
        public UnityEvent OnInteractionCanceled => onInteractionCanceled;

        private void Update()
        {
            if (isInteracting)
            {
                interactionTimer -= Time.deltaTime;

                if (interactionTimer <= 0f)
                {
                    CompleteInteraction();
                }
            }
        }

        public new bool TryInteract(IInteractable newInteractable)
        {
            if (isInteracting || base.TryInteract(newInteractable))
                return false;

            StartInteraction();
            return true;
        }

        private void StartInteraction()
        {
            isInteracting = true;
            interactionTimer = interactionDuration;
            onInteractionStart.Invoke();
        }

        private void CompleteInteraction()
        {
            isInteracting = false;
            onInteractionEnd.Invoke();
            base.Release();
        }

        public new void Release()
        {
            if (isInteracting)
            {
                CancelInteraction();
            }
            else
            {
                base.Release();
            }
        }

        public bool TryCancelInteraction()
        {
            if (!isInteracting)
                return false;

            CancelInteraction();
            return true;
        }

        private void CancelInteraction()
        {
            isInteracting = false;
            interactionTimer = 0f;
            onInteractionCanceled.Invoke();
            base.Release();
        }
    }
}