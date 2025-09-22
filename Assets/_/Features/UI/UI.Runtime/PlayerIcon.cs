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

                duckIcon.SetActive(animalType == AnimalType.DUCK && !gold);
                rabbitIcon.SetActive(animalType == AnimalType.RABBIT && !gold);
                pigIcon.SetActive(animalType == AnimalType.PIG && !gold);
                cowIcon.SetActive(animalType == AnimalType.COW && !gold);
                duckGoldIcon.SetActive(animalType == AnimalType.DUCK && gold);
                rabbitGoldIcon.SetActive(animalType == AnimalType.RABBIT && gold);
                pigGoldIcon.SetActive(animalType == AnimalType.PIG && gold);
                cowGoldIcon.SetActive(animalType == AnimalType.COW && gold);
            

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
