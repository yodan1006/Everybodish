using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Machine.Runtime
{
    public class CommandeManager : MonoBehaviour
    {
        [Header("Paramètres Commandes")]
        [SerializeField] private RecetteUI[] recettesDisponibles;
        [SerializeField] private int maxCommandes = 5;
        [SerializeField] private float tempsEntreCommandes = 5f;
        [SerializeField] private float startY = -300f;
        [SerializeField] private float offsetY = 120f;

        [Header("UI")]
        [SerializeField] private Transform parentUI;

        private readonly List<CommandeUI> commandesActives = new List<CommandeUI>();

        private void Start()
        {
            StartCoroutine(SpawnCommandesRoutine());
        }

        private IEnumerator SpawnCommandesRoutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(tempsEntreCommandes);

                if (commandesActives.Count < maxCommandes)
                {
                    AjouterCommandeAleatoire();
                }
            }
        }

        private void AjouterCommandeAleatoire()
        {
            if (recettesDisponibles.Length == 0) return;

            RecetteUI recette = recettesDisponibles[Random.Range(0, recettesDisponibles.Length)];

            // On instancie directement le prefab lié à la recette
            GameObject uiObj = Instantiate(recette.prefabUI, parentUI, false);
            CommandeUI ui = uiObj.GetComponent<CommandeUI>();
            ui.Initialiser(recette, this);

            commandesActives.Insert(0, ui); // arrive en bas
            ReorganiserCommandes();
        }

        public bool VerifierCommande(FoodType type)
        {
            for (int i = 0; i < commandesActives.Count; i++)
            {
                if (commandesActives[i].Recette.foodType == type)
                {
                    commandesActives[i].Disparaitre();
                    return true;
                }
            }
            return false;
        }

        public void SupprimerCommande(CommandeUI commande)
        {
            commandesActives.Remove(commande);
            ReorganiserCommandes();
        }

        private void ReorganiserCommandes()
        {
            // float startY = -300f;
            // float offsetY = 120f;

            for (int i = 0; i < commandesActives.Count; i++)
            {
                Vector3 targetPos = new Vector3(0, startY + (i * offsetY), 0);
                commandesActives[i].SetTargetPosition(targetPos);
            }
        }
    }
}
