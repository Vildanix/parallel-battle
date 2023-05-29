using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridTogglePattern))]
public class GridTogglePatternEditor : Editor
{
    private SerializedProperty patternProperty;
    private int patternSize = 3; // Default pattern size

    private void OnEnable()
    {
        patternProperty = serializedObject.FindProperty("pattern");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Toggle Pattern", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        patternSize = EditorGUILayout.IntField("Pattern Size", patternSize);

        EditorGUILayout.Space();

        EditorGUILayout.HelpBox("Click on the cells to toggle their state.", MessageType.Info);
        GUILayout.Space(10f);

        for (int y = patternSize - 1; y >= 0; y--)
        {
            GUILayout.BeginHorizontal();

            for (int x = 0; x < patternSize; x++)
            {
                bool currentValue = patternProperty.GetArrayElementAtIndex(y * patternSize + x).boolValue;

                EditorGUI.BeginChangeCheck();
                bool newValue = GUILayout.Toggle(currentValue, GUIContent.none, GUILayout.Width(20), GUILayout.Height(20));
                if (EditorGUI.EndChangeCheck())
                {
                    patternProperty.GetArrayElementAtIndex(y * patternSize + x).boolValue = newValue;
                }
            }

            GUILayout.EndHorizontal();
        }

        serializedObject.ApplyModifiedProperties();
    }
}