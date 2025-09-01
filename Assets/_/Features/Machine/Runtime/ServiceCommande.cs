using System;
using UnityEngine;

namespace Machine.Runtime
{
    public class ServiceCommande : MonoBehaviour
    {
        [SerializeField] private Transform _transformPlat;
        private bool _onService;
        [SerializeField] private float timeurDispawn = 2f; // Valeur par défaut
        private float _timer = 0;
        private GameObject _foodToDestroy = null;

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
                    _timer = timeurDispawn;
                }
            }
        }

        public void ServicePlat(Food food)
        {
            food.gameObject.transform.position = _transformPlat.position;
            _onService = true;
            _timer = timeurDispawn; // On relance le timer
            _foodToDestroy = food.gameObject; // On garde la référence pour la destruction
        }
    }
}