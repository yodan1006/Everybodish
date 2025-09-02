using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    [RequireComponent(typeof(PlayerTeleporter))]
    public class SpawnSystem : MonoBehaviour
    {
        #region Publics
        public float respawnTime = 5;
        public float respawnTimeDelta;
        private PlayerTeleporter playerTeleporter;

        [SerializeField] private Transform m_playerRoot;
        [SerializeField] private GameObject m_player;
        [SerializeField] private Transform m_ragdollRoot;
        [SerializeField] private GameObject m_ragdoll;

        #endregion


        #region Unity Api

        private void Awake()
        {
            playerTeleporter = GetComponent<PlayerTeleporter>();
        }

        // Update is called once per frame
        protected void Update()
        {
            if (respawnTimeDelta > 0)
            {
                respawnTimeDelta -= Time.deltaTime;
                if (respawnTimeDelta <= 0)
                {

                    m_player.SetActive(true);        
                    m_ragdoll.SetActive(true);   
                    playerTeleporter.TeleportTo(transform);
                }
            }
        }

        #endregion


        #region Main Methods
        public void KillPlayer()
        {
            m_player.SetActive(false);
            m_ragdoll.SetActive(false);
            respawnTimeDelta = respawnTime;

        }

        public void KillPlayerNoRespawn()
        {
            m_player.SetActive(false);
            m_ragdoll.SetActive(false);
        }

        #endregion


        #region Utils

        #endregion


        #region Private and Protected

        #endregion


    }
}
