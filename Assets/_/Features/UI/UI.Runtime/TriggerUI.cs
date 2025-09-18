using UnityEngine;

namespace UI.Runtime
{
    public class TriggerUI : MonoBehaviour
    {

        [SerializeField] private GameObject beActivated;
        private bool activated;

        private void Update()
        {
            if (activated)
                beActivated.SetActive(true);
            else
                beActivated.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                activated = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            activated = false;
        }
    }
}
