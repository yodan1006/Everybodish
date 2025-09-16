using UnityEngine;

namespace UI.Runtime
{
    public class PlayerRank : MonoBehaviour
    {
        #region Publics
        public GameObject first;
        public GameObject second;
        public GameObject third;
        public GameObject fourth;
        #endregion


        #region Unity Api

        public void SetRankIcon(int rank)
        {
            gameObject.SetActive(true);
            first.SetActive(rank == 0);
            second.SetActive(rank == 1);
            third.SetActive(rank == 2);
            fourth.SetActive(rank == 3);
        }

        #endregion


        #region Main Methods

        #endregion


        #region Utils

        #endregion


        #region Private and Protected

        #endregion


    }
}
