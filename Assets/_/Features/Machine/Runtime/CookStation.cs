using System.Collections.Generic;
using UnityEngine;

namespace Machine.Runtime
{
    public class CookStation : MonoBehaviour
    {
        [SerializeField] private Recipe[] recipes;
        [SerializeField] private Transform spawnPoint; // pour le plat final
        [SerializeField] private Transform foodSlot;   // pour poser l’ingrédient

        private Food currentFood;

        public bool TryCook(Food food, out GameObject resultPrefab)
        {
            if (currentFood != null)
            {
                resultPrefab = null;
                return false;
            }

            foreach (var recipe in recipes)
            {
                if (recipe.input == food.FoodType)
                {
                    currentFood = food;

                    // Place l’objet visuellement sur le slot
                    food.transform.position = foodSlot.position;
                    food.transform.rotation = foodSlot.rotation;
                    food.transform.SetParent(foodSlot);

                    // Détruit après une frame (ou tu peux déclencher via animation plus tard)
                    //Destroy(currentFood.gameObject);
                    currentFood = null;

                    resultPrefab = recipe.outputPrefab;
                    return true;
                }
            }

            resultPrefab = null;
            return false;
        }

        public void SpawnCookedFood(GameObject prefab)
        {
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
