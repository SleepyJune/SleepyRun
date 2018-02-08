using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

using UnityEditor;

using System.Linq;

[CustomEditor(typeof(TextScriptDatabase))]
public class TextScriptDatabaseGenerator : Editor
{
    SerializedProperty databaseProperty;

    TextScriptDatabase database;

    private void OnEnable()
    {
        databaseProperty = serializedObject.FindProperty("allTexts");

        database = target as TextScriptDatabase;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Generate"))
        {
            EditorHelperFunctions.GenerateFromAsset("/Prefabs/UI/Tutorial/Text", ref database.allTexts, target, "*.asset");
        }
        EditorGUILayout.PropertyField(databaseProperty, true);

        serializedObject.ApplyModifiedProperties();
    }
}