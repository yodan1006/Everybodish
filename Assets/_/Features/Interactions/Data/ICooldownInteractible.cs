namespace Interactions.Data
{
    public interface ICooldownInteractible : IInteractible
    {
        void OnCooldownStart();
        void OnCooldownEnd();
    }
}
