using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Machine.Runtime
{
    public class CookStationMultiIngredient : MonoBehaviour
    {
        [SerializeField] private MultiIngredientRecipe[] recipes;
        [SerializeField] private Transform output;

        private readonly List<Food> _storedFoods = new List<Food>();

        public void AddFood(Food food)
        {
            _storedFoods.Add(food);
            food.gameObject.SetActive(false);
            TryCook();
        }

        private void TryCook()
        {
            foreach (var recipe in recipes)
            {
                if (recipe.IngredientsInput.All(ingredient => _storedFoods.Any(f => f.FoodType == ingredient)))
                {
                    foreach (var ingredient in recipe.IngredientsInput)
                    {
                        var foodToRemove = _storedFoods.FirstOrDefault(f => f.FoodType == ingredient);
                        if (foodToRemove != null)
                        {
                            _storedFoods.Remove(foodToRemove);
                            Destroy(foodToRemove.gameObject);
                        }
                    }
                    SpawnCookedFood(recipe.OutputPrefab);
                    return;
                }
            }
        }

        private void SpawnCookedFood(GameObject prefab)
        {
            Instantiate(prefab, output.position, Quaternion.identity);
        }
    }
}