using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovePlayer.Runtime
{
    [RequireComponent(typeof(Rigidbody))]
    public class Move : MonoBehaviour
    {
        private Vector2 _move;
        private Rigidbody _rb;

        #region proto

        public float angleHeadButt = 20f;   // Inclinaison max vers l'avant
        public float speedHeatButt = 5f;    // Vitesse du mouvement
        private Quaternion _startRot;
        private Quaternion _endRot;

        #endregion
        
        
        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private Collider zoneAttack;

        [SerializeField]private float _timeResetAttack;
        private bool _onFrameAttack;
        private float _timeToAttackFrame;

        [SerializeField]private Animator animator;
        
        
        private void Awake()
        {
            zoneAttack.enabled = false;
            _rb = GetComponent<Rigidbody>();
            PlayerInput playerInput = GetComponent<PlayerInput>();
            foreach (var map in playerInput.actions.actionMaps)
            {
                if (map.name != "Player")
                    map.Disable();
            }
            playerInput.actions.FindActionMap("Player").Enable();
        }

        private void Start()
        {
            //proto
            
            _startRot = transform.rotation;
            _endRot = _startRot * Quaternion.Euler(angleHeadButt, 0f, 0f);
            
            //proto
        }

        private void Update()
        {
            Vector3 move = new(_move.x, 0f, _move.y);
            Vector3 input = new Vector3(_move.x, 0f, _move.y);

            if (input.sqrMagnitude > 0.0001f)
            {
                Vector3 dir = input.normalized;
                Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
                Quaternion newRot = Quaternion.Slerp(_rb.rotation, targetRot, rotationSpeed * Time.fixedDeltaTime);
                _rb.MoveRotation(newRot);
            }

            
            _rb.MovePosition(transform.position + speed * Time.deltaTime * move);

            if (_onFrameAttack)
            {
                _timeToAttackFrame += Time.deltaTime;

                float halfDuration = _timeResetAttack / 2f;
                float t;

                if (_timeToAttackFrame <= halfDuration)
                {
                    // Aller
                    t = _timeToAttackFrame / halfDuration;
                    transform.rotation = Quaternion.Lerp(_startRot, _endRot, t);
                }
                else
                {
                    // Retour
                    t = (_timeToAttackFrame - halfDuration) / halfDuration;
                    transform.rotation = Quaternion.Lerp(_endRot, _startRot, t);
                }

                if (_timeToAttackFrame >= _timeResetAttack)
                {
                    transform.rotation = _startRot;
                    _onFrameAttack = false;
                    _timeToAttackFrame = 0f;
                    AttackDisable();
                }
            }
            
            if(_timeToAttackFrame >= _timeResetAttack) AttackDisable();
        }

        public void MovePlayer(InputAction.CallbackContext context)
        {
            _move = context.ReadValue<Vector2>();
            animator.SetFloat("speedMove", _move.magnitude);
        }
        
        public void Attack(InputAction.CallbackContext context)
        {
            //animation coup de tete
            if (context.started)
            {
                AttackEnable();
                _onFrameAttack = true;
                // Sauvegarde la rotation actuelle
                _startRot = transform.rotation;

                // Ajoute une inclinaison vers l'avant selon l'orientation actuelle
                _endRot = _startRot * Quaternion.Euler(angleHeadButt, 0f, 0f);
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
