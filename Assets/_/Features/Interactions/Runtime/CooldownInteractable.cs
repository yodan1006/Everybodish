using DebugBehaviour.Runtime;
using Interactions.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Interactions.Runtime
{
    [RequireComponent(typeof(Interactable))]
    public class CooldownInteractable : VerboseMonoBehaviour, ICooldownInteractable
    {
        private IInteractable _interactable;

        [SerializeField]
        private float cooldownTime = 0.1f;
        private float cooldownDeltaTime = 0;
        private bool isOnCooldown = false;
        private readonly UnityEvent onCooldownStart = new();
        private readonly UnityEvent cooldownEndEvent = new();

        public UnityEvent OnCooldownStart => onCooldownStart;

        public UnityEvent OnCooldownEnd => cooldownEndEvent;

        public bool IsOnCooldown { get => isOnCooldown; }

        public bool CanInteract => throw new System.NotImplementedException();

        public IInteractor Interactor => _interactable.Interactor;

        public IInteractable Interactable => _interactable;

        private void Awake()
        {
            _interactable = GetComponent<Interactable>();
        }

        private void Update()
        {
            if (IsOnCooldown)
            {
                cooldownDeltaTime -= Time.deltaTime;
                if (cooldownDeltaTime < 0)
                {
                    cooldownEndEvent.Invoke();
                    isOnCooldown = false;
                }
            }
        }


        void IInteractable.Release()
        {
            _interactable.Release();
            isOnCooldown = true;
            cooldownDeltaTime = cooldownTime;
            onCooldownStart.Invoke();
        }

        bool IInteractable.TryInteract(IInteractor newGrabber)
        {
            bool success = false;
            if (cooldownDeltaTime < 0)
            {
                success = _interactable.TryInteract(newGrabber);
            }
            return success;
        }

        public bool IsInteracted()
        {
            return _interactable.IsInteracted();
        }

        public void StartInteraction()
        {
            throw new System.NotImplementedException();
        }
    }
}
