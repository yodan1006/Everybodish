using UnityEngine.Events;

namespace Interactions.Data
{
    public interface ICooldownInteractable : IInteractable
    {
        UnityEvent OnCooldownStart { get; }
        UnityEvent OnCooldownEnd { get; }
    }
}
