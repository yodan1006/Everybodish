using System.Collections.Generic;
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
        private void Awake()
        {
            // Debug.Log("Running MeshBinder", this);
            var allBones = root.GetComponentsInChildren<Transform>();
            var filteredBones = new List<Transform>();

            foreach (var bone in allBones)
            {
                if (!bone.tag.StartsWith("ExtraBone"))  // ignore bones starting with "ExtraBone"
                {
                    filteredBones.Add(bone);
                }
            }

            skinMesh.bones = filteredBones.ToArray();
        }

    }
}
