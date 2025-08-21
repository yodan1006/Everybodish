using ActionMapManager.Data;
using UnityEngine.InputSystem;

namespace ActionMapManager.Runtime
{
    public static class ActionMapManager
    {
        public static void SetActionMap(PlayerInput playerInput, ActionMapTypeEnum actionMapType)
        {
            switch (actionMapType)
            {
                case ActionMapTypeEnum.Lobby:
                    SetLobbyActionMap(playerInput);
                    break;
                case ActionMapTypeEnum.Player:
                    SetPlayerActionMap(playerInput);
                    break;
                default:
                    //leave as is
                    break;
            }

        }

        private static void SetPlayerActionMap(PlayerInput playerInput)
        {
            foreach (var map in playerInput.actions.actionMaps)
            {
                if (map.name != "Player")
                    map.Disable();
            }
            playerInput.actions.FindActionMap("Player").Enable();
        }

        private static void SetLobbyActionMap(PlayerInput playerInput)
        {
            foreach (var map in playerInput.actions.actionMaps)
            {
                if (map.name != "Lobby")
                    map.Disable();
            }
            playerInput.actions.FindActionMap("Lobby").Enable();
        }
    }
}
