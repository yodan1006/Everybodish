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
            prefab = null;
            if (_foodPrefab == null)
                return false;
            prefab = _foodPrefab;
            return true;
        }

        #endregion
        
        #region private

        [SerializeField] private GameObject _foodPrefab;
        [SerializeField] private Transform _foodSpawnPoint;

        #endregion
    }
}