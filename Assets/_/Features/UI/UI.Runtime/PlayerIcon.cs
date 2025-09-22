using Animals.Data;
using UnityEngine;

namespace UI.Runtime
{
    [DisallowMultipleComponent]
    public class PlayerIcon : MonoBehaviour
    {
        #region Publics
        public GameObject duckIcon;
        public GameObject rabbitIcon;
        public GameObject pigIcon;
        public GameObject cowIcon;

        public GameObject duckGoldIcon;
        public GameObject rabbitGoldIcon;
        public GameObject pigGoldIcon;
        public GameObject cowGoldIcon;

        public GameObject player1;
        public GameObject player2;
        public GameObject player3;
        public GameObject player4;
        #endregion



        #region Main Methods
        public void SetPlayerIcon(AnimalType animalType, bool gold)
        {
            if (!gold)
            {
                duckIcon.SetActive(animalType == AnimalType.DUCK);
                rabbitIcon.SetActive(animalType == AnimalType.RABBIT);
                pigIcon.SetActive(animalType == AnimalType.PIG);
                cowIcon.SetActive(animalType == AnimalType.COW);
            }
            else
            {
                duckGoldIcon.SetActive(animalType == AnimalType.DUCK);
                rabbitGoldIcon.SetActive(animalType == AnimalType.RABBIT);
                pigGoldIcon.SetActive(animalType == AnimalType.PIG);
                cowGoldIcon.SetActive(animalType == AnimalType.COW);
            }

        }

        public void SetPlayerLabel(int label)
        {
            player1.SetActive(label == 0);
            player2.SetActive(label == 1);
            player3.SetActive(label == 2);
            player4.SetActive(label == 3);
        }
        #endregion



    }
}
