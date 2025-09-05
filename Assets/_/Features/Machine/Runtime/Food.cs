using Grab.Runtime;
using UnityEngine;

namespace Machine.Runtime
{
    public class Food : MonoBehaviour
    {
        public FoodType FoodType;

        public Rigidbody rb;
        public Grabable grabable;
        public Transform topmost;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            grabable = GetComponent<Grabable>();
            topmost = transform.root;
        }
    }
}