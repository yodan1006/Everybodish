using ActiveRagdoll.Runtime;
using UnityEngine;

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

        #endregion


        #region Utils

        #endregion


        #region Private and Protected
        private PlayerStat stat;
        private Stun stun;
        #endregion


    }
}
