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
        public bool hideItemDuringPrepare = false;
        private Food _currentFood;

        public bool TryCook(Food food, out GameObject resultPrefab)
        {
            if (food == null)
            {
                Debug.LogWarning("TryCook called with null food!");
                resultPrefab = null;
                return false;
            }

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
                        if (food.GetComponent<IngredientTimer>() != null && food.GetComponent<IngredientTimer>().enabled == true)
                            food.GetComponent<IngredientTimer>().enabled = false;
                        _isCooking = true;
                        _currentFood = food;

                        if (hideItemDuringPrepare == true)
                        {
                            if (food.Grabable != null)
                            {
                                food.Grabable.enabled = false;
                            }
                            else
                            {
                                Debug.LogWarning($"CookStation: food.grabable is null on {food.name}");
                            }
                            food.transform.localScale = Vector3.zero;
                        }
                        else
                        {
                            food.gameObject.SetActive(false);
                            // Place l’objet visuellement sur le slot
                            food.transform.SetPositionAndRotation(foodSlot.position, foodSlot.rotation);
                            food.gameObject.SetActive(true);
                            Debug.LogError("Test");
                            if (food.Grabable != null)
                            {
                                food.Grabable.enabled = false;
                            }
                            else
                            {
                                Debug.LogWarning($"CookStation: food.grabable is null on {food.name}");
                            }

                            if (food.Rb != null)
                            {
                                food.Rb.linearVelocity = Vector3.zero;
                                food.Rb.angularVelocity = Vector3.zero;
                            }
                            else
                            {
                                Debug.LogWarning($"CookStation: food.rb (Rigidbody) is null on {food.name}");
                            }
                        }

                        animator.SetBool("OnSlice", true);
                        resultPrefab = recipe.outputPrefab;
                        food.onFoodCooked.Invoke();

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

            switch (_currentFood.FoodType)
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
            currentFood.Topmost.GetComponentInChildren<PlayerStat>().KillPlayer();
        }

        public static void DestroyFoodPrefab(Food food)
        {
            if (food.Topmost.gameObject != food.gameObject)
            {
                GameObject topmostGo = food.Topmost.gameObject;
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
