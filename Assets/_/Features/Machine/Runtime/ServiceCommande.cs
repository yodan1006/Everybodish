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

        [Header("Référence Commandes")] [SerializeField]
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
                        Destroy(_foodToDestroy);
                        _foodToDestroy = null;
                    }

                    _onService = false;
                }
            }
        }

        public void ServicePlat(Food food)
        {
            food.transform.position = _transformPlat.position;
            _onService = true;
            _timer = timeurDispawn;
            _foodToDestroy = food.gameObject;

            if (commandeManager != null)
            {
                bool valide = commandeManager.VerifierCommande(food.FoodType);
                Debug.Log(valide ? "Commande validée !" : "Plat non commandé.");
            }
        }
    }
}