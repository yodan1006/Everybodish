using UnityEngine;
using UnityEngine.InputSystem;

namespace MovePlayer.Runtime
{
    [RequireComponent(typeof(Rigidbody))]
    public class Move : MonoBehaviour
    {
        private Vector2 _move;
        private Rigidbody _rb;

        [SerializeField] private float _speed;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Vector3 move = new(_move.x, 0f, _move.y);
            _rb.MovePosition(transform.position + _speed * Time.deltaTime * move);
        }

        public void MovePlayer(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }
    }
}
