using UnityEngine;

namespace InteractionPlayerStart.Runtime
{
    public class SuppUIOnJoin : MonoBehaviour
    {
        [SerializeField] private GameObject[] UIpressJoin;

        public void OnJoin(int playerIndex)
        {
            UIpressJoin[playerIndex].SetActive(false);
        }

        public void OnLeave(int playerIndex)
        {
            UIpressJoin[playerIndex].SetActive(true);
        }
    }
}
