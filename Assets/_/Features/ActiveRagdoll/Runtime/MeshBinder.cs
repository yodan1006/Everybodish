using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class MeshBinder : MonoBehaviour
    {
        #region Publics
        public GameObject root;
        public SkinnedMeshRenderer skinMesh;
        #endregion


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            skinMesh.bones = root.GetComponentsInChildren<Transform>();
        }

    }
}
