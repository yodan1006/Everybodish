using UnityEngine;
using UnityEngine.InputSystem;

namespace MovePlayer.Runtime
{
    [RequireComponent(typeof(Rigidbody))]
    public class Move : MonoBehaviour
    {
        private Vector2 _move;
        private Rigidbody _rb;

        [SerializeField] private float speed;
        [SerializeField] private Collider zoneAttack;

        private bool _onFrameAttack;
        private float _timeToAttackFrame;
        private float _time;
        private void Awake()
        {
            zoneAttack.enabled = false;
            _rb = GetComponent<Rigidbody>();
            PlayerInput playerInput = GetComponent<PlayerInput>();
            foreach (var map in playerInput.actions.actionMaps)
            {
                if (map.name != "lobby")
                    map.Disable();
            }
            playerInput.actions.FindActionMap("lobby").Enable();
        }

        private void Update()
        {
            Vector3 move = new(_move.x, 0f, _move.y);
            _rb.MovePosition(transform.position + speed * Time.deltaTime * move);
            
            if(_onFrameAttack)_timeToAttackFrame += Time.deltaTime;
            
            if(_timeToAttackFrame >= _time) AttackDisable();
        }

        public void MovePlayer(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
        }

        public void Attack(InputAction.CallbackContext context)
        {
            //animation coup de tete
            if (context.started)
            {
                AttackEnable();
                _onFrameAttack = true;
            };
            
        }

        public void AttackEnable()
        {
            zoneAttack.enabled = true;
        }

        public void AttackDisable()
        {
            zoneAttack.enabled = false;
        }
    }
}
