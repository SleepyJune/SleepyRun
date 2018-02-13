using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

using UnityEditor;

using System.Linq;

[CustomEditor(typeof(TextScript))]
public class TextScriptEditor : Editor
{
    SerializedProperty textProperty;
    TextScript textScript;

    private void OnEnable()
    {
        textProperty = serializedObject.FindProperty("text");
        textScript = target as TextScript;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorStyles.textField.wordWrap = true;
        textProperty.stringValue = EditorGUILayout.TextArea(textProperty.stringValue, GUILayout.MaxHeight(75));

        serializedObject.ApplyModifiedProperties();
    }
}