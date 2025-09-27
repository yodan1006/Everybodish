using MovePlayer.Runtime;
using StunSystem.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Stunsystem.Runtime
{
    [RequireComponent(typeof(PlayerStat))]
    public class DamageReceiver : MonoBehaviour
    {
        public float stunDuration = 5;
        
        #region Publics
        public void TakeDamage(int damage)
        {
            Debug.Log("Took damage!");
            stat.HurtPlayer(damage);
            if(hitEffect != null)
                hitEffect.Play();
            animator.SetTrigger("Hit");
            
            // Déclencher la vibration de la manette
            TriggerControllerVibration();
            
            if (stat.CurrentLife() < 0)
            {
                stun.StunForDuration(stunDuration);
                stat.ResetLife();
            }
        }
        #endregion

        #region Unity Api
        private void Awake()
        {
            stat = GetComponent<PlayerStat>();
            stun = GetComponent<Stun>();
        }
        #endregion

        #region Main Methods
        private void TriggerControllerVibration()
        {
            // Obtenir la manette spécifique à ce joueur
            if (Gamepad.all.Count > playerIndex)
            {
                var playerInput = GetComponentInParent<PlayerInput>();
                if (playerInput != null && playerInput.devices.Count > 0)
                {
                    var gamepad = playerInput.devices[0] as Gamepad;
                    if (gamepad != null)
                    {
                        gamepad.SetMotorSpeeds(vibrationIntensity, vibrationIntensity);
                        Invoke(nameof(StopVibration), vibrationDuration);
                    }
                }

            }
        }

        private void StopVibration()
        {
            if (Gamepad.all.Count > playerIndex)
            {
                var gamepad = Gamepad.all[playerIndex];
                if (gamepad != null)
                {
                    gamepad.SetMotorSpeeds(0f, 0f);
                }
            }
        }

        #endregion

        #region Utils
        #endregion

        #region Private and Protected
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem hitEffect;
        [SerializeField] private float vibrationDuration = 0.1f; // Durée de la vibration
        [SerializeField] private float vibrationIntensity = 0.8f; // Intensité de la vibration (0-1)
        private PlayerStat stat;
        private Stun stun;
        private int playerIndex = 0;

        #endregion
    }
}