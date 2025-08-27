using UnityEngine;

namespace Machine.Runtime
{
    public class FoodPickup : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerInventory inventory))
            {
                if (!inventory.HasFood)
                {
                    Food food = GetComponent<Food>();
                    inventory.PickUp(food);
                    Debug.Log($"Le joueur a ramassé : {food.FoodType}");
                }
            }
        }
    }
}