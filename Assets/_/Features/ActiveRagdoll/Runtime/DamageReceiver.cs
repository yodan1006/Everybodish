using ActiveRagdoll.Runtime;
using UnityEngine;

namespace MovePlayer.Runtime
{
    [RequireComponent(typeof(PlayerStat))]
    public class DamageReceiver : MonoBehaviour
    {
        public float stunDuration = 5;
        private float stunDurationDelta = 0;
        #region Publics
        public void TakeDamage(int damage)
        {
            Debug.Log("Took damage!");
            stat.HurtPlayer(damage);
            if (stat.CurrentLife() < 0)
            {
                stun.enabled = true;
                stunDurationDelta = stunDuration;
            }
        }
        #endregion


        #region Unity Api

        private void Awake()
        {
            stat = GetComponent<PlayerStat>();
            stun = GetComponent<Stun>();
        }

        private void Update()
        {
            if (stunDurationDelta > 0)
            {
                stunDurationDelta -= Time.deltaTime;
                if (stunDurationDelta <= 0)
                {
                    stun.enabled = false;
                    stat.ResetLife();
                }
            }
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
