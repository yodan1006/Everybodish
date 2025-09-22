using UnityEngine;

namespace UI.Runtime
{
    public class UiMoveUpDawn : MonoBehaviour
    {
        [SerializeField] private float amplitude = 1f;   // Force/hauteur de l’oscillation
        [SerializeField] private float frequency = 1f;   // Vitesse de l’oscillation
        [SerializeField] private float lerpSpeed = 0.05f;

        private Vector3 startPosition;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            float offsetY = Mathf.Sin(Time.time * frequency) * amplitude;
            Vector3 targetPosition = new Vector3(startPosition.x, startPosition.y + offsetY, startPosition.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed);
        }
    }
}