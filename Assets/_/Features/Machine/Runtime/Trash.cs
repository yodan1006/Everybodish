using UnityEngine;

namespace Machine.Runtime._.Features.Machine.Runtime
{
    public class Trash : MonoBehaviour
    {
        public bool ThrowFood(Food food)
        {
            if (food == null) return false;

            // Détruire proprement l'objet
            CookStation.DestroyFoodPrefab(food);

            // // Feedback visuel
            // if (throwEffect != null)
            //     throwEffect.Play();
            //
            // // Feedback sonore
            // if (destroySound != null)
            //     destroySound.Play();

            Debug.Log($"Food {food.name} jeté à la poubelle.");
            return true;
        }
    }
}