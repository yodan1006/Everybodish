using System;
using System.Collections.Generic;
using Score.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionPlayerStart.Runtime
{
    public class ResetScore : MonoBehaviour
    {
        private readonly Dictionary<int, PlayerInput> players = new();
        private void Awake()
        {
            GlobalScoreEventSystem.ResetAllScores(players.Values);
            foreach (PlayerInput player in players.Values)
            {
                player.actions.FindActionMap("Player").Enable();
                player.GetComponent<InputMapManager.Runtime.InputMapSwitcher>().SetGameplayMap();
            }   
        }
    }
}
