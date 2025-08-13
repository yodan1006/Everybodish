using Interactions.Data;

namespace Interactions.Runtime
{
    public class CooldownInteractible : Interactible, ICooldownInteractible
    {
        public void OnCooldownEnd()
        {
            throw new System.NotImplementedException();
        }

        public void OnCooldownStart()
        {
            throw new System.NotImplementedException();
        }
    }
}
