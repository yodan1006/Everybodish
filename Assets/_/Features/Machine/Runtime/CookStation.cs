using System.Collections.Generic;
using UnityEngine;

namespace Machine.Runtime
{
    public class CookStation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem particle;
        
        [SerializeField] private Recipe[] recipes;
        [SerializeField] private Transform spawnPoint; // pour le plat final
        [SerializeField] private Transform foodSlot;   // pour poser l’ingrédient

        private Food _currentFood;

        public bool TryCook(Food food, out GameObject resultPrefab)
        {
            if (_currentFood != null)
            {
                resultPrefab = null;
                return false;
            }

            foreach (var recipe in recipes)
            {
                if (recipe.input == food.FoodType)
                {
                    _currentFood = food;

                    // Place l’objet visuellement sur le slot
                    food.transform.position = foodSlot.position;
                    food.transform.rotation = foodSlot.rotation;
                    food.transform.SetParent(foodSlot);

                    animator.SetBool("OnSlice", true);
                    //Destroy(currentFood.gameObject);
                    //_currentFood = null;

                    resultPrefab = recipe.outputPrefab;
                    return true;
                }
            }

            resultPrefab = null;
            return false;
        }

        // --- Animation Events ---
        public void ActivateParticuleSysteme()
        {
            if (particle != null)
            {
                particle.Play();
                Debug.Log("Particule jouée");
            }
        }

        public void DestroyObjectInStation()
        {
            if (_currentFood != null)
            {
                Debug.Log("DestroyObjectInStation appelé");
                Destroy(_currentFood.gameObject);
                // Debug.Log("Objet brut détruit : " + _currentFood.name);
                // NE PAS mettre _currentFood = null ici
            }
        }

        public void SpawnCookedFood()
        {
            if (_currentFood == null)
            {
                Debug.LogWarning("SpawnCookedFood : _currentFood est null !");
                return;
            }

            // Sauvegarde locale pour sécuriser
            Food foodToCook = _currentFood;

            // Cherche la recette correspondante
            Recipe recipe = null;
            foreach (var r in recipes)
            {
                if (r.input == foodToCook.FoodType)
                {
                    recipe = r;
                    break;
                }
            }

            if (recipe != null && recipe.outputPrefab != null)
            {
                Instantiate(recipe.outputPrefab, spawnPoint.position, Quaternion.identity);
                Debug.Log("Plat cuit spawn : " + recipe.outputPrefab.name);
            }
            else
            {
                Debug.LogWarning("SpawnCookedFood : recette introuvable ou prefab manquant pour " + foodToCook.FoodType);
            }

            // Fin animation et nettoyage
            animator.SetBool("OnSlice", false);
            _currentFood = null;
        }
    }
}
