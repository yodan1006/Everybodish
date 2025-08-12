using UnityEngine;

namespace LobbyInstantiate.Runtime
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoint;
        private int _playerCount;

        public void OnPlayerSpawned()
        {
            Transform spawn = spawnPoint[_playerCount % spawnPoint.Length];
            
            Instantiate(Resources.Load<GameObject>("Player"), spawn.position, spawn.rotation);
            
            _playerCount++;
        }
    }
}
