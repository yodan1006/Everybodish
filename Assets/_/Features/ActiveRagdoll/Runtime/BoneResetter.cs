using UnityEngine;
using UnityEngine.UIElements;

namespace ActiveRagdoll.Runtime
{
    public class BoneResetter : MonoBehaviour
    {

        Quaternion rotation;
        private Vector3 position;

        private void Awake()
        {
            rotation = transform.rotation;
            position = transform.localPosition;
        }

        private void OnEnable()
        {
            transform.rotation = rotation;
            transform.localPosition = position;
        }
    }
}
