using UnityEngine;

namespace Machine.Runtime
{
    public class SlideOnMachine : MonoBehaviour
    {
        [SerializeField] private Vector3 slideDirection = Vector3.forward; // direction locale de glissement
        [SerializeField] private float slideForce = 5f;

        private void OnCollisionStay(Collision collision)
        {
            // Cherche le Rigidbody dans l'objet ou ses enfants
            Rigidbody rb = collision.collider.GetComponentInParent<Rigidbody>();
            if (rb == null)
                rb = collision.collider.GetComponentInChildren<Rigidbody>();

            if (rb != null && !rb.isKinematic)
            {
                // Applique la force dans la direction souhaitée
                rb.AddForce(slideDirection.normalized * slideForce, ForceMode.Acceleration);
            }
        }
    }
}
