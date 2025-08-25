using UnityEngine;

namespace Machine.Runtime
{
    [ System.Serializable]
    public class FoodRecipe
    {
        public FoodType input;      // Aliment de base
        public GameObject result;   // Prefab de sortie
        public float cookTime = 2f; // Temps de cuisson / préparation NTA
    }
}