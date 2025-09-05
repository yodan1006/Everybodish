using UnityEngine;

namespace ActiveRagdoll.Runtime
{
    public class MeshBinder : MonoBehaviour
    {
        #region Publics
       public GameObject root;
      public  SkinnedMeshRenderer skinMesh;
        #endregion
        
        
        #region Unity Api
        
            // Start is called once before the first execution of Update after the MonoBehaviour is created
            void Start()
            {
            skinMesh.bones = root.GetComponentsInChildren<Transform>();
        }
        
            // Update is called once per frame
            void Update()
            {
        
            }
        
        #endregion
        
        
        #region Main Methods
        
        #endregion
        
        
        #region Utils
        
        #endregion
        
        
        #region Private and Protected
        
        #endregion
        
        
    }
}
