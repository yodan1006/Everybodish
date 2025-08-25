using UnityEngine;

namespace Machine.Runtime
{
    [CreateAssetMenu(menuName = "Features/Recipe")]
    public class Recipe : ScriptableObject
    {
        public FoodType input;
        public FoodType output;
        public GameObject outputPrefab;
    }
}