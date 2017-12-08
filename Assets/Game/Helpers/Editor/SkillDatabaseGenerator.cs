using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

using UnityEditor;

using System.Linq;

[CustomEditor(typeof(SkillDatabase))]
public class SkillDatabaseGenerator : Editor
{
    SerializedProperty databaseProperty;

    SkillDatabase database;

    private void OnEnable()
    {
        databaseProperty = serializedObject.FindProperty("allSkills");

        database = target as SkillDatabase;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Generate"))
        {
            EditorHelperFunctions.GenerateFromAsset("/Prefabs/Skills", ref database.allSkills, target, "*.prefab");
        }
        EditorGUILayout.PropertyField(databaseProperty, true);

        serializedObject.ApplyModifiedProperties();
    }
}