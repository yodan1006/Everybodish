using System.Collections.Generic;
using UnityEngine;

namespace Machine.Runtime
{
    [CreateAssetMenu(menuName = "Features/MultiIngredientRecipe")]
    public class MultiIngredientRecipe : ScriptableObject
    {
        public List<FoodType> IngredientsInput;
        public FoodType Output;
        public GameObject OutputPrefab;
    }
}