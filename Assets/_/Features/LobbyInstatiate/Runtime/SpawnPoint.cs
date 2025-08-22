using UnityEngine;
using UnityEngine.InputSystem;

namespace LobbyInstantiate.Runtime
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoint;
        private int _playerCount;

        public void OnPlayerSpawned(PlayerInput playerInput)
        {
            Transform spawn = spawnPoint[_playerCount % spawnPoint.Length];

            playerInput.transform.position = spawn.transform.position;
            playerInput.transform.rotation = spawn.transform.rotation;

            _playerCount++;
        }
    }
}
