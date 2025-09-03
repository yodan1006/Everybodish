using UnityEngine;

namespace Machine.Runtime
{
    public class CommandeUI : MonoBehaviour
    {
        private RectTransform rectTransform;
        private Vector3 targetPosition;
        private float moveSpeed = 10f;

        public RecetteUI Recette { get; private set; }
        private CommandeManager manager;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        // ⚠️ c’est cette méthode que ton manager appelle
        public void Initialiser(RecetteUI recette, CommandeManager manager)
        {
            Recette = recette;
            this.manager = manager;

            // Spawn hors écran à gauche
            rectTransform.anchoredPosition = new Vector2(-600, -300);
        }

        public void SetTargetPosition(Vector3 pos)
        {
            targetPosition = pos;
        }

        private void Update()
        {
            rectTransform.anchoredPosition = Vector3.Lerp(
                rectTransform.anchoredPosition,
                targetPosition,
                Time.deltaTime * moveSpeed
            );
        }

        public void Disparaitre()
        {
            manager.SupprimerCommande(this);
            Destroy(gameObject);
        }
    }
}