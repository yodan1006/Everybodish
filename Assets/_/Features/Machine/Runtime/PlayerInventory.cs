using UnityEngine;

namespace Machine.Runtime
{
    public class PlayerInventory : MonoBehaviour
    {
        
        private Food carriedFood;

        public bool HasFood => carriedFood != null;
        public Food CarriedFood => carriedFood;

        public void PickUp(Food food)
        {
            if (HasFood) return;

            carriedFood = food;
            //food.gameObject.SetActive(false); // On cache l’objet ramassé
        }

        public Food Drop()
        {
            if (!HasFood) return null;

            Food temp = carriedFood;
            //carriedFood.gameObject.SetActive(true); // On le réactive
            carriedFood = null;
            return temp;
        }
    }
}