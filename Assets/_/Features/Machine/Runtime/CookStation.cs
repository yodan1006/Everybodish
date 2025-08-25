using System.Collections.Generic;
using UnityEngine;

namespace Machine.Runtime
{
    public class CookStation : MonoBehaviour
    {
        [SerializeField] private Recipe[] recipes;
        [SerializeField] private Transform spawnPoint;

        public bool TryCook(Food food, out GameObject resultPrefab)
        {
            foreach (var recipe in recipes)
            {
                if (recipe.input == food.FoodType)
                {
                    resultPrefab = recipe.outputPrefab;
                    Destroy(food.gameObject); // Supprime l’ancien aliment
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
