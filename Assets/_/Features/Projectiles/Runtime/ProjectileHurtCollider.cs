using Grab.Runtime;
using StunSystem.Runtime;
using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class ProjectileHurtCollider : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        [SerializeField] private Stun _stun;
        [SerializeField] private Grabber _grab;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float minmimumStunVelocity = 5;
        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                //Check for collision while grabbing
                if (!_grab.IsGrabbing() || _grab.Grabable.gameObject != collision.gameObject)
                {
                    //Check for speed and not being grabbed
                    if ((rb.linearVelocity - _characterController.velocity).magnitude > minmimumStunVelocity)
                    {
                        if (collision.TryGetComponent<Grabable>(out Grabable grabable))
                        {
                            if (!grabable.IsGrabbed())
                            {
                                if(grabable.LastGrabber.gameObject != _grab.gameObject)
                                {
                                Debug.Log("Player stunned by grabable object!", this);
                                _stun.StunForDuration(5);
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("Player stunned by kinetic object!", this);
                            _stun.StunForDuration(5);
                        }
                    }else
                    {
                        Debug.Log("Projectile was too slow to hurt");
                    }
                } else
                {
                    Debug.Log("Item was throw by the same player, aborting");
                }
                   

            }
            else
            {
                Debug.Log("Rigidbody Receiver not found : " + collision.gameObject.name, this);
            }
        }

        private void Awake()
        {
            if (_collider != null)
            {
                _collider.GetComponent<Collider>();
            }
        }
    }
}
