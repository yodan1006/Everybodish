using UnityEngine;

namespace MovePlayer.Runtime
{
    public class AttackTrigger : MonoBehaviour
    {
         [SerializeField] private int degat;
        [SerializeField] private Collider _collider;
         private void OnTriggerEnter(Collider other)
         {
            if(TryGetComponent<DamageReceiver>(out DamageReceiver receiver))
            {
                receiver.TakeDamage( degat);
            }
         }

        private void Awake()
        {
            _collider.GetComponent<Collider>();
        }

        private void OnEnable()
        {
            _collider.enabled = true;
        }

        private void OnDisable()
        {
            _collider.enabled=false;
        }
    }
}
