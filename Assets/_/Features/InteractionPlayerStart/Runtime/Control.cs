using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionPlayerStart.Runtime
{
    public class Control : MonoBehaviour
    {
        // Exemple de nom d'objet à rechercher, peut aussi venir d'une variable publique/exportée
        [SerializeField] private string nameMenuControl;
        [SerializeField] private string nameMenuCredit;

        public void ControleMenu(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameObject objet = GameObject.Find(nameMenuControl);
                if (objet != null)
                {
                    objet.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Objet non trouvé : " + nameMenuControl);
                }
            }
        }

        public void CreditMenu(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameObject objet = GameObject.Find(nameMenuCredit);
                if (objet != null)
                {
                    objet.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Objet non trouvé : " + nameMenuCredit);
                }
            }
        }

        public void Quitter(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                Application.Quit();
            }
        }

    }
}