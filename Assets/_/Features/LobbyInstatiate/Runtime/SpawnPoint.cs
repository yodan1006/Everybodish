using ActiveRagdoll.Runtime;
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

            string deviceName = "Unknown Device";
            if (playerInput.user != null && playerInput.user.pairedDevices.Count > 0)
            {
                InputDevice device = playerInput.user.pairedDevices[0];
                deviceName = device.displayName ?? device.name;
            }

            PlayerTeleporter teleporter = playerInput.GetComponentInChildren<PlayerTeleporter>();
            if (teleporter != null)
            {
                teleporter.TeleportTo(spawn);
                playerInput.transform.root.name = deviceName + " Player";
            }
            else
            {
                // fallback si jamais pas de teleporter
                playerInput.transform.SetPositionAndRotation(spawn.position, spawn.rotation);
                playerInput.transform.name = deviceName + " Player";
            }

            // if (playerInput.transform.parent != null)
            // {
            //     if (playerInput.transform.parent.TryGetComponent<PlayerTeleporter>(out PlayerTeleporter teleporter))
            //     {
            //         teleporter.TeleportTo(spawn);
            //     }
            //     playerInput.transform.parent.name = deviceName + " Player";
            // }
            // else
            // {
            //     playerInput.transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
            //     playerInput.transform.name = deviceName + " Player";
            // }
        }
    }
}
