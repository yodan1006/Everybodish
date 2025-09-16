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
        #endregion



        #region Main Methods
        public void SetPlayerIcon(AnimalType animalType)
        {
            duckIcon.SetActive(animalType == AnimalType.DUCK);
            rabbitIcon.SetActive(animalType == AnimalType.RABBIT);
            pigIcon.SetActive(animalType == AnimalType.PIG);
            cowIcon.SetActive(animalType == AnimalType.COW);
        }
        #endregion



    }
}
