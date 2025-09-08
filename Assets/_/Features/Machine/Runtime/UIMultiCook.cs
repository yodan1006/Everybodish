using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Machine.Runtime
{
    public class UIMultiCook : MonoBehaviour
    {
        [SerializeField] private Canvas uiCanvas;
        [SerializeField] public Image uiImage;

        [SerializeField] private float holdTime = 1f;
        
        public UnityEvent OnHoldComplete;
        
        private bool _isHolding;
        private bool _isVisible;
        private float _holdTimerProgress;

        private void Awake()
        {
            if (uiCanvas != null) uiCanvas.enabled = false;
            if (uiImage != null) uiImage.fillAmount = 0;
        }

        private void Update()
        {
            if (!_isHolding) return;
            
            _holdTimerProgress += Time.deltaTime / holdTime;
            if (uiImage != null) uiImage.fillAmount = _holdTimerProgress;
            
            if (_holdTimerProgress >= 1f)
            {
                OnHoldComplete?.Invoke();
                ResetHold();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (gameObject.TryGetComponent<CookStationMultiIngredient>(out var cookStationMultiIngredient))
                if (cookStationMultiIngredient.IsCooking)
                {
                    return;
                }
            if (other.TryGetComponent<PlayerInteract>(out var player))
            {
                ShowUI();
                player.CurrentMultiCookUI = this;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerInteract>(out var player))
            {
                HideUI();
                player.CurrentMultiCookUI = null;
            }
        }

        public void ShowUI()
        {
            if (uiCanvas != null) uiCanvas.enabled = true;
            if (uiImage!= null) uiImage.fillAmount = 0;
            _isVisible = true;
        }
        
        public void HideUI()
        {
            if (uiCanvas != null) uiCanvas.enabled = false;
            ResetHold();
            _isVisible = false;
        }

        public void StartHold()
        {
            if (!_isVisible) return;
            _isHolding = true;
            _holdTimerProgress = 0f;
            if (uiImage != null) uiImage.fillAmount = 0;
        }

        public void StopHold()
        {
            ResetHold();
        }
        
        private void ResetHold()
        {
            _isHolding = false;
            _holdTimerProgress = 0f;
            if (uiImage != null) uiImage.fillAmount = 0;

        }
    }
}
