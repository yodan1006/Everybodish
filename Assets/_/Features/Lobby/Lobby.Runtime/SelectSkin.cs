using Animals.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Skins.Runtime
{
    public class SelectSkin : MonoBehaviour
    {
        #region public

        public UnityEvent<bool> onSkinValidated = new();

        /// <summary>
        /// Retourne le type d'animal actuellement sélectionné par le joueur.
        /// </summary>
        public AnimalType CurrentAnimalType()
        {
            return animalTypes[currentModelIndex];
        }

        /// <summary>
        /// Permet d'attribuer l'index du slot attribué au joueur.
        /// </summary>
        public void AssignSlotIndex(int index) => mySlotIndex = index;

        /// <summary>
        /// Permet de récupérer l'index du slot attribué au joueur.
        /// </summary>
        public int GetSlotIndex() => mySlotIndex;

        /// <summary>
        /// Indique si le joueur a validé son skin (prêt).
        /// </summary>
        public bool IsReady { get; private set; } = false;

        #endregion

        #region unity api

        /// <summary>
        /// Initialise l'objet au lancement.
        /// Ajoute le joueur au LobbyManager, choisit une apparence aléatoire et l'applique.
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            if (LobbyManager.Instance != null)
            {
                LobbyManager.Instance.RegisterPlayer(this);
            }
            // Apparence aléatoire à l'entrée
            currentModelIndex = Random.Range(0, appearances.Length);
            ApplyAppearance();
        }

        #endregion

        #region main method

        /// <summary>
        /// Change le modèle sélectionné (animal) selon l'entrée du joueur.
        /// Met aussi à jour l'apparence affichée.
        /// </summary>
        public void OnChangeModel(InputAction.CallbackContext context)
        {
            if (IsReady) return;
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

        /// <summary>
        /// Change la couleur du modèle sélectionné selon les entrées du joueur.
        /// Met à jour l'apparence affichée.
        /// </summary>
        public void OnChangeColor(InputAction.CallbackContext context)
        {
            if (IsReady) return;
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

        /// <summary>
        /// Action de validation du skin par le joueur.
        /// Marque le joueur comme prêt et met à jour l'UI, puis vérifie si tous les joueurs sont prêts.
        /// </summary>
        public void OnValidateSkin(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (IsReady) return;

            IsReady = true;
            Debug.Log("✅ Joueur a validé, tentative CheckAllReady...");

            if (LobbyManager.Instance != null)
            {
                int slotIndex = GetSlotIndex();
                if (slotIndex >= 0 && slotIndex < LobbyManager.Instance.UiValidate.Length)
                {
                    LobbyManager.Instance.UiValidate[slotIndex].SetActive(true);
                    LobbyManager.Instance.UiAButton[slotIndex].SetActive(false);
                    LobbyManager.Instance.UiReady[slotIndex].SetActive(false);
                }

                LobbyManager.Instance.CheckAllReady();

            }
            else
            {
                Debug.LogError("❌ LobbyManager.Instance est NULL !");
            }
        }

        #endregion

        #region utils

        /// <summary>
        /// Applique l'apparence courante : rend le modèle et la couleur actifs selon les index actuels,
        /// en désactivant toutes les autres variantes d'apparence.
        /// </summary>
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

        #endregion

        #region private
        
        [SerializeField] private ApparenceSet[] appearances;
        [SerializeField] private AnimalType[] animalTypes;
        [SerializeField] private float changeCooldown = 0.2f;
        private float lastModelChangeTime = 0f;
        private float lastColorChangeTime = 0f;
        private int currentModelIndex = 0;
        private int currentColorIndex = 0;
        private int mySlotIndex = -1;

        #endregion
    }
}