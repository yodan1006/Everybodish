using System;
using MovePlayer.Runtime;
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

        public bool _isCooking = false;
        private Food _currentFood;

        public bool TryCook(Food food, out GameObject resultPrefab)
        {

            if (_currentFood != null)
            {
                resultPrefab = null;
                return false;
            }
            if (_isCooking != true)
            {
                foreach (var recipe in recipes)
                {
                    if (recipe.input == food.FoodType)
                    {
                        _isCooking = true;
                        _currentFood = food;

                        // Place l’objet visuellement sur le slot
                        food.transform.transform.SetPositionAndRotation(foodSlot.transform.position, foodSlot.transform.rotation);

                        //food.rb.isKinematic = true;
                        food.grabable.enabled = false;
                        food.rb.linearVelocity = Vector3.zero;
                        food.rb.angularVelocity = Vector3.zero;
                        animator.SetBool("OnSlice", true);
                        resultPrefab = recipe.outputPrefab;
                        return true;
                    }
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
            if (_currentFood == null)
            {
                Debug.LogWarning("DestroyObjectInStation appelé mais _currentFood est null !");
                return;
            }

            Debug.Log("DestroyObjectInStation appelé pour " + _currentFood.name);

            switch(_currentFood.FoodType)
            {
                case FoodType.Player:
                    KillPlayer(_currentFood);
                    break;
                default:
                    DestroyFoodPrefab(_currentFood);
                    break;
            }

    
        }

        private void KillPlayer(Food currentFood)
        {
            currentFood.topmost.GetComponentInChildren<PlayerStat>().KillPlayer();
        }

        public static void DestroyFoodPrefab(Food food)
        {
            if (food.topmost.gameObject != food.gameObject)
            {
                GameObject topmostGo = food.topmost.gameObject; 
                //Destroy the reparented food item
                Destroy(food.gameObject);
                //Destroy topmost item that might be lost in the scene
                Destroy(topmostGo);

            }
            else
            {
                Destroy(food.gameObject);
            }
            food = null;
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
            animator.SetBool("OnSlice", false);
        }

        private void EndingRecipe()
        {
            _isCooking = false;
        }
    }
}
