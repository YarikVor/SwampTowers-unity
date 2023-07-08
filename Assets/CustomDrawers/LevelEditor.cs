#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor"))
        {
            var level = (Level)target;
            LevelEditorWindow.OpenWindow(level);
        }

        base.OnInspectorGUI();
    }
}
#endif