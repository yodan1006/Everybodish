using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Grab.Runtime
{
    public class DragAndDrop : RigidbodyGrabber
    {
        [SerializeField] protected LayerMask dragToAreaLayers;
        [SerializeField] protected float maxDragDistance = 10f;
        [SerializeField] protected float targetDistanceFromFloor = 1.0f;
        [SerializeField] protected Texture2D cursorTextureDefault;
        [SerializeField] protected Vector2 hotSpotDefault = Vector2.zero;
        [SerializeField] protected Texture2D cursorTextureEnabled;
        [SerializeField] protected Vector2 hotSpotEnabled = Vector2.zero;
        [SerializeField] protected CursorStates currentState = CursorStates.Default;
        public enum CursorStates
        {
            Default,
            Enabled
        }

        public CursorMode cursorMode = CursorMode.Auto;

        private OutlineContainer lastHoveredContainer = null;


        private void OnEnable()
        {
            Cursor.SetCursor(cursorTextureDefault, hotSpotDefault, cursorMode);
        }

        private new void Update()
        {
            base.Update();
        }

        private void OnMouseOver()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxGrabRange))
            {
                GameObject go = hit.collider.gameObject;
                SetOutLine(go, true);

                if (go.TryGetComponent<Grabable>(out Grabable grab))
                {
                    SetCursorState(CursorStates.Enabled);
                }
                else
                {
                    SetCursorState(CursorStates.Default);
                }
            }
            else
            {
                SetCursorState(CursorStates.Default);
            }
        }

        private void OnMouseExit()
        {
            lastHoveredContainer.EnableOutline(false);
            lastHoveredContainer = null;
        }

        private void SetOutLine(GameObject go, bool enable)
        {
            if (go.TryGetComponent<OutlineContainer>(out OutlineContainer OLcontainer))
            {
                OLcontainer.EnableOutline(enable);
                lastHoveredContainer = OLcontainer;
            }
        }

        private void SetCursorState(CursorStates state)
        {
            if (state != currentState)
            {
                currentState = state;
                switch (currentState)
                {
                    case CursorStates.Enabled:
                        Cursor.SetCursor(cursorTextureEnabled, hotSpotEnabled, cursorMode);
                        break;
                    case CursorStates.Default:
                        Cursor.SetCursor(cursorTextureDefault, hotSpotDefault, cursorMode);
                        break;
                }
            }

        }

        private void FixedUpdate()
        {
            if (IsGrabbing())
            {
                UpdateTargetPosition();
                MoveObject();
            }
        }

        public void OnGrab(CallbackContext context)
        {

            if (context.performed == true)
            {
                Log("Starting Drag & Drop", this);
                StartDrag();
            }
            else if (context.canceled == true)
            {
                Release();
            }
        }

        public void StartDrag()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxGrabRange))
            {
                Rigidbody rb = hit.rigidbody;
                if (hit.rigidbody != null)
                {
                    if (rb.gameObject.TryGetComponent<Grabable>(out Grabable grab))
                    {
                        if (TryGrab(grab))
                        {
                            Log("Cursor Drag success", this);
                            UpdateTargetPosition();
                        }
                        else
                        {
                            Log("Cursor Drag failure", this);
                        }
                    }
                    else
                    {
                        Log("Grabable not found", this);
                    }
                }
                else
                {
                    Log("Hit on non grabbable object detected", this);
                }
            }
        }
        protected void UpdateTargetPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDragDistance, dragToAreaLayers);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDragDistance, dragToAreaLayers))
            {
                Debug.Log("Hit: " + hit.collider.name);
            }
            else
            {
                Debug.Log("No hit.");
            }


            if (hits.Length > 0)
            {
                float closestHitDistance = Vector3.Distance(hits[0].point, transform.position);
                RaycastHit closestHit = hits[0];
                foreach (RaycastHit hit2 in hits)
                {
                    if (Grabable.gameObject != hit.rigidbody.gameObject)
                    {
                        float hitDistance = Vector3.Distance(hit.point, transform.position);
                        if (hitDistance < closestHitDistance)
                        {
                            closestHitDistance = hitDistance;
                            closestHit = hit;
                        }
                    }
                }
                Vector3 targetPosition = closestHit.point;
                targetPosition.y += targetDistanceFromFloor;
                target.transform.position = targetPosition;
            } else
            {
                Log("No hit found for drag to target", this);
            }
        }
    }

}
