using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class BoneResetter : MonoBehaviour
    {

        private Quaternion rotation;
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
