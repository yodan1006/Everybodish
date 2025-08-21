using UnityEditor;
using UnityEngine;

namespace StructuredAssemblyCreator.Editor
{
    public class PopupEditor : EditorWindow
    {
        private float TimeToLive;
        private string popupText;

        public void ShowPopup(float time, string text)
        {
            TimeToLive = time;
            popupText = text;
        }

        private void OnGUI()
        {
            TimeToLive -= Time.deltaTime;
            EditorGUILayout.LabelField(popupText);
            if (TimeToLive < 0)
            {
                Close();
            }
        }
    }

}


