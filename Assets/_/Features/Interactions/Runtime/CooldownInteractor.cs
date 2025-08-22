using DebugBehaviour.Runtime;
using Interactions.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Interactions.Runtime
{
    [RequireComponent(typeof(Interactor))]
    public class CooldownInteractor : VerboseMonoBehaviour, ICooldownInteractor
    {
        public UnityEvent onInteractionAvailable = new();
        public UnityEvent onInteractionUnavailable = new();
        public UnityEvent onInteractionSuccess = new();
        public UnityEvent onInteractionFail = new();
        private IInteractor _interactor;
        public UnityEvent OnInteractionAvailable { get => onInteractionAvailable; }
        public UnityEvent OnInteractionUnavailable { get => onInteractionUnavailable; }
        public UnityEvent OnInteractionSuccess { get => onInteractionUnavailable; }
        public UnityEvent OnInteractionFail { get => onInteractionUnavailable; }
        public IInteractable Interactable { get => _interactor.Interactable; }



        private void Awake()
        {
            _interactor = GetComponent<Interactor>();
        }

        public void Release()
        {

        }

        public bool TryInteract(IInteractor newInteractor)
        {
            return false;
        }

        public bool CanInteract()
        {
            throw new System.NotImplementedException();
        }

        public bool IsInteracting()
        {
            throw new System.NotImplementedException();
        }

        public bool TryInteract(IInteractable newInteractable)
        {
            throw new System.NotImplementedException();
        }

        public void StopInteraction()
        {
            throw new System.NotImplementedException();
        }
    }
}
