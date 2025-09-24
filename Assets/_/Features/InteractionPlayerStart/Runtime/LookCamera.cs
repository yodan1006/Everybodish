using UnityEngine;

namespace InteractionPlayerStart.Runtime
{
    public class LookCamera : MonoBehaviour
    {
        /// <summary>
        /// LookCamera : Ce composant permet de faire en sorte que le GameObject auquel il est attaché regarde en permanence une caméra désignée.
        /// Il récupère la référence du GameObject à l'initialisation et, à chaque frame, oriente ce GameObject en direction de la caméra ciblée.
        /// </summary>

        [SerializeField] private Camera _camera;
        private GameObject _gameObject;

        private void Start()
        {
            _gameObject = gameObject;
        }
        private void Update()
        {
            _gameObject.transform.LookAt(_camera.transform.position);
        }
    }
}
