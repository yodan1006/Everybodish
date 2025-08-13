using System;
using UnityEngine;

namespace MovePlayer.Runtime
{
    public class TriggerAttack : MonoBehaviour
    {
        [SerializeField] private int degat;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            other.GetComponent<PlayerStat>().m_life -= degat;
        }
    }
}
