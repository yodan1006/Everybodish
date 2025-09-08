using Grab.Runtime;
using Machine.Runtime;
using MovePlayer.Runtime;
using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class KillArea : MonoBehaviour
    {
        public CookStation station;
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.TryGetComponent<Food>(out Food food))
            {
                if(station != null)
                {
                    station.TryCook(food,out GameObject _);
                }
                Debug.Log("Player killed!", this);
            }
            else
            {
                Debug.Log("Damage Receiver not found : " + collider.gameObject.name, this);
            }
        }

    }
}
