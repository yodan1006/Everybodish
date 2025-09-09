using Machine.Runtime;
using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class KillArea : MonoBehaviour
    {
        public CookStation station;
        private void OnTriggerEnter(Collider collider)
        {
            ChopIngredient(collider);
        }

        private void OnTriggerStay(Collider collider)
        {
            ChopIngredient(collider);
        }

        public void ChopIngredient(Collider collider)
        {
            if (collider.gameObject.TryGetComponent<Food>(out Food food))
            {
                if (station != null)
                {
                    if (station.TryCook(food, out GameObject _))
                    {
                        Debug.Log("Item choppped!", this);
                    }
                }
                else
                {
                    Debug.Log("Station is null, aborting", this);
                }
            }
            else
            {
                Debug.Log("This is not an ingredient.", this);
            }
        }
    }
}
