using UnityEngine;
using UnityEngine.InputSystem;

namespace MovePlayer.Runtime
{
    public class SelectSkin : MonoBehaviour
    {
        public ApparenceSet[] appearances;

        [SerializeField] private float changeCooldown = 0.2f;
        private float lastModelChangeTime = 0f;
        private float lastColorChangeTime = 0f;
        private int currentModelIndex = 0;
        private int currentColorIndex = 0;
        private readonly bool isInitialized = false;

        public void OnChangeModel(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (Time.time - lastModelChangeTime < changeCooldown) return;

            float value = context.ReadValue<Vector2>().x;
            if (value > 0.5f) currentModelIndex++;
            else if (value < -0.5f) currentModelIndex--;
            else return;

            if (appearances == null || appearances.Length == 0) return;

            currentModelIndex = (currentModelIndex + appearances.Length) % appearances.Length;
            currentColorIndex = 0;

            lastModelChangeTime = Time.time;

            ApplyAppearance();
        }

        public void OnChangeColor(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (Time.time - lastColorChangeTime < changeCooldown) return;

            if (appearances == null || appearances.Length == 0) return;

            var currentSet = appearances[currentModelIndex];
            if (currentSet == null || currentSet.colorAppaerences == null || currentSet.colorAppaerences.Length == 0) return;

            float value = context.ReadValue<Vector2>().y;
            if (value > 0.5f) currentColorIndex++;
            else if (value < -0.5f) currentColorIndex--;
            else return; // pas assez de mouvement

            int colorCount = currentSet.colorAppaerences.Length;
            currentColorIndex = (currentColorIndex + colorCount) % colorCount;

            lastColorChangeTime = Time.time;

            ApplyAppearance();
        }

        private void ApplyAppearance()
        {
            // Désactive toutes les variantes
            foreach (var set in appearances)
            {
                foreach (var variant in set.colorAppaerences)
                    variant.SetActive(false);
            }
            // Active la variante sélectionnée
            appearances[currentModelIndex].colorAppaerences[currentColorIndex].SetActive(true);
        }
    }
}
