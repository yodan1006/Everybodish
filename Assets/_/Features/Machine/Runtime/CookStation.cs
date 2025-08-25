using System.Collections.Generic;
using UnityEngine;

namespace Machine.Runtime
{
    public class CookStation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private List<FoodRecipe> recipes;

        private bool isInUse = false;
        private GameObject currentResultPrefab;

        public void TryCook(PlayerInventory inventory)
        {
            if (isInUse) return;
            if (!inventory.HasFood) return;

            Food food = inventory.TakeFood();
            Debug.Log($"La station reçoit : {food.FoodType}");

            // Chercher une recette qui correspond à l’aliment
            FoodRecipe recipe = recipes.Find(r => r.input == food.FoodType);

            if (recipe != null)
            {
                currentResultPrefab = recipe.result;
                animator.SetTrigger("StartCooking");
                StartCoroutine(CookRoutine(recipe.cookTime));
            }

            Destroy(food.gameObject); // on enlève l’aliment brut
            isInUse = true;
        }

        private System.Collections.IEnumerator CookRoutine(float time)
        {
            yield return new WaitForSeconds(time);

            // Spawn du prefab transformé
            if (currentResultPrefab != null)
            {
                Instantiate(currentResultPrefab, transform.position + Vector3.up, Quaternion.identity);
            }

            animator.SetTrigger("EndCooking");
            isInUse = false;
        }
    }
}
