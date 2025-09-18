using Animals.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Skins.Runtime
{
    public class SelectSkin : MonoBehaviour
    {
        [SerializeField] private ApparenceSet[] appearances;
        [SerializeField] private AnimalType[] animalTypes;

        [SerializeField] private float changeCooldown = 0.2f;
        private float lastModelChangeTime = 0f;
        private float lastColorChangeTime = 0f;

        private int currentModelIndex = 0;
        private int currentColorIndex = 0;

        public UnityEvent<bool> onSkinValidated = new();

        private int mySlotIndex = -1;
        public void AssignSlotIndex(int index) => mySlotIndex = index;
        public int GetSlotIndex() => mySlotIndex;

        // Nick Modification
        [Header("Color Lock (par Poste/Slot)")]
        [SerializeField] private bool lockColorBySlot = true;
        [SerializeField] private int[] colorIndexPerSlot;
        private int forcedColorIndex = -1;

        public AnimalType CurrentAnimalType()
        {
            return (animalTypes != null && animalTypes.Length > 0)
                ? animalTypes[Mathf.Clamp(currentModelIndex, 0, animalTypes.Length - 1)]
                : default;
        }

        public bool IsReady { get; private set; } = false;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (LobbyManager.Instance != null)
                LobbyManager.Instance.RegisterPlayer(this);

            if (appearances != null && appearances.Length > 0)
                currentModelIndex = Random.Range(0, appearances.Length);

            forcedColorIndex = ResolveForcedColorIndex();
            currentColorIndex = forcedColorIndex;

            ApplyAppearance();
        }

        private int ResolveForcedColorIndex()
        {
            int slot = GetSlotIndex();
            int fallback = Mathf.Max(0, slot);

            int colorCount = 0;
            if (appearances != null && appearances.Length > 0)
            {
                var set = appearances[Mathf.Clamp(currentModelIndex, 0, appearances.Length - 1)];
                if (set != null && set.colorAppaerences != null)
                    colorCount = set.colorAppaerences.Length;
            }
            if (colorCount == 0) return 0;

            int chosen =
                (colorIndexPerSlot != null &&
                 slot >= 0 &&
                 slot < colorIndexPerSlot.Length)
                ? colorIndexPerSlot[slot]
                : fallback;

            return ((chosen % colorCount) + colorCount) % colorCount;
        }

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

            if (lockColorBySlot)
            {
                var set = appearances[currentModelIndex];
                int colorCount = (set != null && set.colorAppaerences != null) ? set.colorAppaerences.Length : 0;
                currentColorIndex = (colorCount > 0)
                    ? ((forcedColorIndex % colorCount) + colorCount) % colorCount
                    : 0;
            }
            else
            {
                currentColorIndex = 0;
            }

            lastModelChangeTime = Time.time;
            ApplyAppearance();
        }

        public void OnChangeColor(InputAction.CallbackContext context)
        {
            if (lockColorBySlot) return;

            if (IsReady) return;
            if (!context.performed) return;
            if (Time.time - lastColorChangeTime < changeCooldown) return;

            if (appearances == null || appearances.Length == 0) return;

            var currentSet = appearances[currentModelIndex];
            if (currentSet == null || currentSet.colorAppaerences == null || currentSet.colorAppaerences.Length == 0) return;

            float value = context.ReadValue<Vector2>().y;
            if (value > 0.5f) currentColorIndex++;
            else if (value < -0.5f) currentColorIndex--;
            else return;

            int colorCount = currentSet.colorAppaerences.Length;
            currentColorIndex = (currentColorIndex + colorCount) % colorCount;

            lastColorChangeTime = Time.time;
            ApplyAppearance();
        }

        private void ApplyAppearance()
        {
            if (appearances == null || appearances.Length == 0) return;

            foreach (var set in appearances)
            {
                if (set == null || set.colorAppaerences == null) continue;
                foreach (var variant in set.colorAppaerences)
                    if (variant != null) variant.SetActive(false);
            }

            int safeModel = Mathf.Clamp(currentModelIndex, 0, appearances.Length - 1);
            var setNow = appearances[safeModel];
            if (setNow != null && setNow.colorAppaerences != null && setNow.colorAppaerences.Length > 0)
            {
                int colorCount = setNow.colorAppaerences.Length;

                int colorToUse = lockColorBySlot
                    ? ((forcedColorIndex % colorCount) + colorCount) % colorCount
                    : ((currentColorIndex % colorCount) + colorCount) % colorCount;

                currentColorIndex = colorToUse;

                var go = setNow.colorAppaerences[colorToUse];
                if (go != null) go.SetActive(true);
            }
        }

        public void OnValidateSkin(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            if (IsReady) return;

            IsReady = true;

            if (LobbyManager.Instance != null)
            {
                int slotIndex = GetSlotIndex();
                if (slotIndex >= 0 && slotIndex < LobbyManager.Instance.UIValidate.Length)
                {
                    LobbyManager.Instance.UIValidate[slotIndex].SetActive(true);
                    LobbyManager.Instance.UIA[slotIndex].SetActive(false);
                    LobbyManager.Instance.UIreeady[slotIndex].SetActive(false);
                }

                LobbyManager.Instance.CheckAllReady();
            }
        }
    }
}
