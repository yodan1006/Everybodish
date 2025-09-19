using UnityEngine;

namespace InteractionPlayerStart.Runtime
{
    public class LookCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        private GameObject _gameObject;

        private void Start()
        {
            _gameObject = this.gameObject;
        }
        private void Update()
        {
            _gameObject.transform.LookAt(_camera.transform.position);
        }
    }
}
