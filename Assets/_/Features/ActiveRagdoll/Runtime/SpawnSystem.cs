using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    [RequireComponent(typeof(PlayerTeleporter))]
    public class SpawnSystem : MonoBehaviour
    {
        #region Publics
        public float respawnTime = 5;
        private float respawnTimeDelta;
        private PlayerTeleporter playerTeleporter;

        [SerializeField] private GameObject m_playerRoot;
        [SerializeField] private GameObject m_ragdollRoot;
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
                    playerTeleporter.TeleportTo(transform);
                }
            }
        }

        #endregion


        #region Main Methods
        public void KillPlayer()
        {
            m_playerRoot.SetActive(false);
            m_ragdollRoot.SetActive(false);
            respawnTimeDelta = respawnTime;
        }

        public void KillPlayerNoRespawn()
        {
            m_playerRoot.SetActive(false);
            m_ragdollRoot.SetActive(false);
        }

        #endregion


        #region Utils

        #endregion


        #region Private and Protected

        #endregion


    }
}
