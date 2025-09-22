using ActiveRagdoll.Runtime;
using Skins.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Spawner.Runtime
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;

        public enum SpawnStrategy
        {
            SpawnByPlayerIndex,
            SpawnByJoinOrder,
            SpawnBySelectSkin
        }

        [SerializeField] private SpawnStrategy strategy = SpawnStrategy.SpawnBySelectSkin;
        private int spawnCount = 0;
        public void OnPlayerSpawned(PlayerInput playerInput)
        {
            if (spawnPoints.Length > 0)
            {
                Transform spawn;

                switch (strategy)
                {
                    case SpawnStrategy.SpawnByPlayerIndex:
                        spawn = spawnPoints[playerInput.playerIndex % spawnPoints.Length];
                        break;
                    case SpawnStrategy.SpawnByJoinOrder:
                        spawn = spawnPoints[spawnCount % spawnPoints.Length];
                        spawnCount++;
                        break;
                    case SpawnStrategy.SpawnBySelectSkin:
                        spawn = spawnPoints[playerInput.GetComponentInChildren<SelectSkin>().GetSlotIndex() % spawnPoints.Length];
                        break;
                    default:
                        Debug.LogError("Unknown spawn strategy");
                        spawn = spawnPoints[0];
                        break;
                }


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
                //   Debug.LogError("Player Teleported");
            }
            else
            {
                Debug.Log("No assigned spawn points!");
            }
        }
    }
}
