using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Machine.Runtime
{
    public class CookStationMultiIngredient : MonoBehaviour
    {
        [SerializeField] private MultiIngredientRecipe[] recipes;
        [Header("Apparence et feedback")]
        [SerializeField] private Transform output;
        [SerializeField] private GameObject poopPrefab;
        [Header("Points d'apparition des ingrédients (ordre important !)")]
        [SerializeField] private Transform[] ingredientPoints;
        [Header("Scales designers par ingrédient (ordre identique aux points)")]
        [SerializeField] private Vector3[] ingredientScales;
        [Header("Paramètres de la poêle")]
        [SerializeField] private int maxIngredients = 4;

        private readonly List<Food> _storedFoods = new();
        private bool _isCooking = false;

        public bool IsCooking => _isCooking;

        public void AddFood(Food food)
        {
            if (_isCooking)
                return; // cuisson en cours !

            if (_storedFoods.Count >= maxIngredients)
                return; // trop d'ingrédients

            if (food == null) return;

            bool isPartOfAnyRecipe = recipes != null &&
                                     recipes.Any(r => r != null &&
                                             r.IngredientsInput != null &&
                                             r.IngredientsInput.Contains(food.FoodType));
            if (!isPartOfAnyRecipe)
            {
                WrongIngredient(food);
                return;
            }

            if (_storedFoods.Contains(food)) return;

            // Remplacement si même type déjà dans la poêle
            var existing = _storedFoods.FirstOrDefault(f => f.FoodType == food.FoodType);
            if (existing != null)
            {
                int index = _storedFoods.IndexOf(existing);
                _storedFoods.RemoveAt(index);
                Destroy(existing.gameObject);
            }


            _storedFoods.Add(food);

            int slotIndex = _storedFoods.Count - 1;
            if (ingredientPoints.Length > slotIndex && ingredientPoints[slotIndex] != null)
            {
                food.gameObject.transform.position = ingredientPoints[slotIndex].position;
                food.gameObject.transform.rotation = ingredientPoints[slotIndex].rotation;
                // Echelle du GD
                if (ingredientScales.Length > slotIndex)
                    food.gameObject.transform.localScale = ingredientScales[slotIndex];
            }

            food.gameObject.SetActive(true); // On les voit dans la poêle !
            TryCook();
        }

        private void TryCook()
        {
            if (_storedFoods.Count == 0) return;

            foreach (var recipe in recipes)
            {
                if (recipe.IngredientsInput.All(ingredient => _storedFoods.Any(f => f.FoodType == ingredient)) &&
                    _storedFoods.Count == recipe.IngredientsInput.Count)
                {
                    _isCooking = true;
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
                    _isCooking = false;
                    return;
                }
            }
            // Si on bloque le stockage sans avoir pu cuisiner -> erreur/caca
            if (_storedFoods.Count >= maxIngredients)
            {
                WrongIngredient(null);
            }
        }

        private void WrongIngredient(Food lastTried)
        {
            // Vide la poêle et spawn le caca
            foreach (var food in _storedFoods)
            {
                if (food != null)
                    Destroy(food.gameObject);
            }
            _storedFoods.Clear();
            if (poopPrefab != null)
                Instantiate(poopPrefab, output.position, Quaternion.identity);
        }

        private void SpawnCookedFood(GameObject prefab)
        {
            if (prefab != null)
                Instantiate(prefab, output.position, Quaternion.identity);
        }

        public void TryManualCook()
        {
            if (_isCooking || _storedFoods.Count == 0)
                return;

            // Recherche une RECETTE correspondant exactement à la sélection
            MultiIngredientRecipe matchedRecipe = null;
            foreach (var recipe in recipes)
            {
                bool allPresent = recipe.IngredientsInput.All(ingredient =>
                    _storedFoods.Any(f => f.FoodType == ingredient));
                bool correctCount = _storedFoods.Count == recipe.IngredientsInput.Count;
                if (allPresent && correctCount)
                {
                    matchedRecipe = recipe;
                    break;
                }
            }

            if (matchedRecipe != null)
            {
                _isCooking = true;
                foreach (var ingredient in matchedRecipe.IngredientsInput)
                {
                    var foodToRemove = _storedFoods.FirstOrDefault(f => f.FoodType == ingredient);
                    if (foodToRemove != null)
                    {
                        _storedFoods.Remove(foodToRemove);
                        Destroy(foodToRemove.gameObject);
                    }
                }
                SpawnCookedFood(matchedRecipe.OutputPrefab);
                _isCooking = false;
            }
            else
            {
                // Erreur = caca !
                WrongIngredient(null);
            }
        }

    }
}