using MovePlayer.Runtime;
using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class ProjectileHurtCollider : MonoBehaviour
    {
        [SerializeField] private int degat = 1;
        [SerializeField] private Collider _collider;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<DamageReceiver>(out DamageReceiver receiver))
            {
                receiver.TakeDamage(degat);
                Debug.Log("Attacked player!");
                Destroy(this);
            }
            else
            {
                Debug.Log("Damage Receiver not found : " + collision.gameObject.name, this);
                Destroy(this);
            }
        }

        private void Awake()
        {
            _collider.GetComponent<Collider>();
        }

    }
}
