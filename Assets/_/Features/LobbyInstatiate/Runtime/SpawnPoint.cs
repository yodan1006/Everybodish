using UnityEngine;
using UnityEngine.InputSystem;

namespace LobbyInstantiate.Runtime
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;

        public void OnPlayerSpawned(PlayerInput playerInput)
        {
            Transform spawn = spawnPoints[playerInput.playerIndex % spawnPoints.Length];

            playerInput.transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
        }
    }
}
