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
            var playerInput = GetComponentInParent<PlayerInput>();
            if (playerInput != null && playerInput.devices.Count > 0)
            {
                // Trouver la première manette associée à ce joueur
                foreach (var device in playerInput.devices)
                {
                    if (device is Gamepad gamepad)
                    {
                        // Arrêter toute vibration précédente sur ce gamepad
                        gamepad.SetMotorSpeeds(0f, 0f);
                        // Sauvegarder la référence pour l'arrêt
                        currentGamepad = gamepad;
                        // Éviter plusieurs Invoke superposés
                        CancelInvoke(nameof(StopVibration));
                        // Lancer la vibration
                        gamepad.SetMotorSpeeds(vibrationIntensity, vibrationIntensity);
                        Invoke(nameof(StopVibration), vibrationDuration);
                        return;
                    }
                }
            }
        }


        private void StopVibration()
        {
            if (currentGamepad != null)
            {
                currentGamepad.SetMotorSpeeds(0f, 0f);
                currentGamepad = null;
            }
        }


        #endregion

        #region Utils
        #endregion

        #region Private and Protected
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem hitEffect;
        [SerializeField] private float vibrationDuration = 0.1f; // Durée de la vibration
        [SerializeField] private float vibrationIntensity = 0.9f; // Intensité de la vibration (0-1)
        private PlayerStat stat;
        private Stun stun;
        private Gamepad currentGamepad;
        private int playerIndex = 0;

        #endregion
    }
}