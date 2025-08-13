namespace Interactions.Data
{
    public interface ITimedInteractible : ICooldownInteractible
    {
        void OnInteractStart();
        void OnInteractEnd();
        void OnInteractCanceled();

    }
}
