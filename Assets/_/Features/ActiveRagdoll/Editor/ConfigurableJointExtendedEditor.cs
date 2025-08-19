using ActiveRagdoll.Runtime;
using UnityEditor;
using UnityEngine;

namespace ActiveRagdoll.Editor
{
    [CustomEditor(typeof(ConfigurableJointExtended))]
    public class ConfigurableJointExtendedEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ConfigurableJointExtended script = (ConfigurableJointExtended)target;

            if (GUILayout.Button("Apply Adaptive Joint Config"))
            {
                script.ApplyAdaptiveConfigInEditor();

                // Ensure changes are saved in edit mode
                if (!Application.isPlaying)
                {
                    EditorUtility.SetDirty(script);
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(script.gameObject.scene);
                }
            }
        }
    }
}
