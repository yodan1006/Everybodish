using UnityEngine;

namespace Machine.Runtime
{
    public class CommandeUI : MonoBehaviour
    {
        private RectTransform rectTransform;

        private Vector2 targetPosition;
        private float moveSpeed = 10f;

        public RecetteUI Recette { get; private set; }
        private CommandeManager manager;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        // Appelé par le CommandeManager
        public void Initialiser(RecetteUI recette, CommandeManager manager)
        {
            Recette = recette;
            this.manager = manager;

            // Spawn hors écran à gauche (par rapport au parent UI)
            rectTransform.anchoredPosition = new Vector2(-800f, -300f);
        }

        public void SetTargetPosition(Vector2 pos)
        {
            targetPosition = pos;
        }

        private void Update()
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
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