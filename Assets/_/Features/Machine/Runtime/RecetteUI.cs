using UnityEngine;

namespace Machine.Runtime
{
    [CreateAssetMenu(fileName = "RecetteUI", menuName = "Features/Recipe/recipeUI")]
    public class RecetteUI : ScriptableObject
    {
        public FoodType foodType;
        public GameObject prefabUI;
    }
}
