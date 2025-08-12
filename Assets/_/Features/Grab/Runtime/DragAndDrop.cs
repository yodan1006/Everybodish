using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Grab.Runtime
{
    public class DragAndDrop : RigidbodyGrabber
    {
        #region Publics

        public Texture2D cursorTextureDefault;
        public Vector2 hotSpotDefault = Vector2.zero;
        public Texture2D cursorTextureEnabled;
        public Vector2 hotSpotEnabled = Vector2.zero;
        public CursorStates currentState = CursorStates.Default;
        public enum CursorStates
        {
            Default,
            Enabled
        }

        public CursorMode cursorMode = CursorMode.Auto;
        #endregion


        #region Unity Api
        private void OnEnable()
        {
            Cursor.SetCursor(cursorTextureDefault, hotSpotDefault, cursorMode);
        }

        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                GameObject go = hit.collider.gameObject;
                SetOutLine(go);

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

        private void SetOutLine(GameObject go)
        {
            if (go.TryGetComponent<OutlineContainer>(out OutlineContainer OLcontainer))
            {
                OLcontainer.EnableOutlineWithTimer();
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

        #endregion


        #region Main Methods
        public void OnGrab(CallbackContext context)
        {

            if (context.performed == true)
            {
                Debug.Log("Grabbed object");
                StartGrab();
            }
            else if (context.canceled == true)
            {
                Release();
            }
        }

        public void StartGrab()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
            {
                if (hit.rigidbody.gameObject.TryGetComponent<Grabable>(out Grabable grab))
                {
                    base.TryGrab(grab);
                    PickupObject(hit.transform.gameObject);
                }
            }
        }
        protected void UpdateTargetPosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            targetPosition.position = ray.GetPoint(holdRange);
        }
        public new void Release()
        {
            base.Release();
            DropObject();
        }
        #endregion

    }

}
