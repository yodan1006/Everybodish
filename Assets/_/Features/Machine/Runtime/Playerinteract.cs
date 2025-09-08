using Grab.Runtime;
using Score.Runtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Machine.Runtime
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerInteract : MonoBehaviour
    {
        // [SerializeField] private Transform holdPoint;
        // private GameObject heldObject;
        private AnimatedProximityGrabber grabber;
        [SerializeField] private float radiusDetector;

        private bool isHoldCooking = false;
        private float holdingTime = 0f;
        [SerializeField] private float holdToCookDuration = 1f;
        private bool cookTriggered = false;
        public UnityEvent<ScoreEventType> onScoreEvent = new();

        private void Awake()
        {
            if (TryGetComponent<AnimatedProximityGrabber>(out grabber))
            {
                Debug.LogError("Grabber not found!");
            }
        }

        private void Update()
        {
            if (isHoldCooking)
            {
                holdingTime += Time.deltaTime;
                if (!cookTriggered && holdingTime >= holdToCookDuration)
                {
                    TryCookMultiIngredientStation();
                    cookTriggered = true;
                }
            }
        }


        private void TryUseCookStation()
        {
            if (!grabber.IsGrabbing()) return;

            if (!grabber.Grabable.gameObject.TryGetComponent<Food>(out Food food))
                return;

            Collider[] hits = Physics.OverlapSphere(transform.position, radiusDetector);

            foreach (var hit in hits)
            {
                // Cas 1 : CookStation simple
                if (hit.TryGetComponent<CookStation>(out var simpleStation) && hit.GetComponent<CookStation>()._isCooking == false)
                {
                    if (simpleStation.TryCook(food, out _))
                    {
                        grabber.Release();
                        if (food.FoodType == FoodType.Player)
                        {
                            onScoreEvent.Invoke(ScoreEventType.PreparedIngredient);
                        }
                        else
                        {
                            onScoreEvent.Invoke(ScoreEventType.PlayerKilled);
                        }

                        //food = null;
                        //simpleStation.SpawnCookedFood(resultPrefab);
                    }

                } // Cas 2 : CookStation multi
                else if (hit.TryGetComponent<CookStationMultiIngredient>(out var multiStation))
                {
                    grabber.Release();
                    multiStation.AddFood(food);

                }
                else if (hit.TryGetComponent<ServiceCommande>(out var serviceCommande))
                {
                    grabber.Release();
                    if (serviceCommande.ServicePlat(food))
                    {
                        onScoreEvent.Invoke(ScoreEventType.ServedDish);
                    }

                }
            }
        }

        private void SpawnAndGrabFood()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radiusDetector);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<Barille>(out var barille))
                {
                    if (!grabber.IsGrabbing() && barille.TryProvideFood(out var newFoodPrefab))
                    {
                        // Spawn et grab direct
                        var newFood = Instantiate(newFoodPrefab, barille.SpawnPoint.position, Quaternion.identity);
                        var grabable = newFood.GetComponentInChildren<Grabable>();
                        if (grabable != null)
                            grabber.TryGrab(grabable);
                    }

                }
                else if (hit.TryGetComponent<CookStationMultiIngredient>(out var multiStation) && multiStation._goReturn)
                {
                    multiStation.PlayRetournerAnimation();
                }
                else if (hit.TryGetComponent<CookStationMultiIngredient>(out var multistation) && multistation._goFinish)
                {
                    multistation.FinishFoodFrying();

                }
            }
        }

        public void OnUse(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (grabber.IsGrabbing()) TryUseCookStation();
                else SpawnAndGrabFood();
            }
        }

        private void TryCookMultiIngredientStation()
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radiusDetector);
            foreach (var hit in hits)
            {
                if (hit.TryGetComponent<CookStationMultiIngredient>(out var multiStation))
                {
                    multiStation.TryManualCook();
                    break; // Un seul appel !
                }
            }
        }


        public void OnManualCook(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                isHoldCooking = true;
                holdingTime = 0f;
                cookTriggered = false;
            }
            if (context.canceled)
            {
                isHoldCooking = false;
                holdingTime = 0f;
                cookTriggered = false;
            }
        }
    }
}
