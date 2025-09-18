using Grab.Runtime;
using UnityEngine;

namespace Machine.Runtime
{
    public class ServiceCommande : MonoBehaviour
    {
        [SerializeField] private Transform _transformPlat;
        [SerializeField] private float timeurDispawn = 2f;
        private float _timer = 0;
        private bool _onService;
        private GameObject _foodToDestroy;

        [Header("Référence Commandes")]
        [SerializeField]
        private CommandeManager commandeManager;

        private void Update()
        {
            if (_onService)
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    if (_foodToDestroy != null)
                    {
                        Destroy(_foodToDestroy.gameObject.transform.parent.parent.gameObject);
                        _foodToDestroy = null;
                    }

                    _onService = false;
                }
            }
        }

        public bool ServicePlat(Food food)
        {
            if (food.FoodType != FoodType.Player)
            {
                
                bool valide = false;
                food.transform.position = _transformPlat.position;
                _onService = true;
                _timer = timeurDispawn;
                _foodToDestroy = food.gameObject;
                food.GetComponent<Grabable>().enabled = false;

                if (commandeManager != null)
                {
                    valide = commandeManager.VerifierCommande(food.FoodType);
                    Debug.Log(valide ? "Commande validée !" : "Plat non commandé.");
                }
                return valide;
            }
            return false;
        }
    }
}