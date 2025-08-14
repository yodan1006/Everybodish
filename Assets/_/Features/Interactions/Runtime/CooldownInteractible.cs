using Interactions.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Interactions.Runtime
{
    public class CooldownInteractible : Interactable, ICooldownInteractible
    {


        [SerializeField]
        private float cooldownTime = 0.1f;
        private float cooldownDeltaTime = 0;
        private bool isOnCooldown = false;
        private UnityEvent cooldownStartEvent = new();
        private UnityEvent cooldownEndEvent = new();

        public UnityEvent onCooldownStart => cooldownStartEvent;

        public UnityEvent onCooldownEnd => cooldownEndEvent;

        public bool IsOnCooldown { get => isOnCooldown; }

        private new void Release()
        {
            base.Release();
            isOnCooldown = true;
            cooldownDeltaTime = cooldownTime;
            cooldownStartEvent.Invoke();
        }
        private new bool TryInteract(IInteractor newGrabber)
        {
            bool success = false;
            if (cooldownDeltaTime < 0)
            {
                success = base.TryInteract(newGrabber);
            }
            return success;
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
    }
}
