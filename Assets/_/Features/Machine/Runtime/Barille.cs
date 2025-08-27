using Grab.Runtime;
using UnityEngine;

namespace Machine.Runtime
{
    public class Barille : MonoBehaviour
    {
        [SerializeField] private GameObject _foodPrefab;
        [SerializeField] private Transform _foodSpawnPoint;
        
        public Transform SpawnPoint => _foodSpawnPoint;

        public bool TryProvideFood(out GameObject prefab)
        {
            prefab = null;

            if (_foodPrefab == null)
                return false;

            prefab = _foodPrefab;
            return true;
        }
    }
}