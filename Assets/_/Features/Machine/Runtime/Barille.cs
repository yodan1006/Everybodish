using UnityEngine;

namespace Machine.Runtime
{
    public class Barille : MonoBehaviour
    {
        #region public

        /// <summary>
        /// Retourne le point de spawn où la nourriture doit apparaître.
        /// </summary>
        public Transform SpawnPoint => _foodSpawnPoint;
        private float cooldownDelta = 0;
        public float cooldownTime = 1f;
        #endregion

        #region Main Methods

        /// <summary>
        /// Tente de fournir le prefab de nourriture associé à ce Barille.
        /// Retourne true si une nourriture est disponible, sinon false.
        /// </summary>
        /// <param name="prefab">Prefab de la nourriture à fournir, ou null si aucun n'est disponible.</param>
        /// <returns>Vrai si nourriture dispo, sinon faux.</returns>
        public bool TryProvideFood(out GameObject prefab)
        {
            bool success  = false;
            prefab = null;

            if (_foodPrefab != null && cooldownDelta <=0)
            {   
                prefab = _foodPrefab;
                success = true;
                cooldownDelta = cooldownTime;
            }
     
            return success;
        }

        private void Update()
        {
            if (cooldownDelta > 0)
            {
                cooldownDelta -= Time.deltaTime;
            }
        }

        #endregion

        #region private

        [SerializeField] private GameObject _foodPrefab;
        [SerializeField] private Transform _foodSpawnPoint;

        #endregion
    }
}