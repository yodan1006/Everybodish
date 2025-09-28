using Grab.Runtime;
using UnityEngine;
using UnityEngine.Events;

namespace Machine.Runtime
{
    public class Food : MonoBehaviour
    {
        public FoodType FoodType;

        private Rigidbody rb;
        private Grabable grabable;
        private Transform topmost;
        public Rigidbody Rb { get => rb; }
        public Grabable Grabable { get => grabable; }
        public Transform Topmost { get => topmost; }
        public UnityEvent onFoodCooked = new();

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            grabable = GetComponent<Grabable>();
            topmost = transform.root;
            rb.constraints = Grabable.ReleaseAreaConstraints;
        }
    }
}