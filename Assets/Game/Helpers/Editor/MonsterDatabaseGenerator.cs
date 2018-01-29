using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

using UnityEditor;

using System.Linq;

[CustomEditor(typeof(MonsterDatabase))]
public class MonsterDatabaseGenerator : Editor
{
    SerializedProperty databaseProperty;

    MonsterDatabase database;

    private void OnEnable()
    {
        databaseProperty = serializedObject.FindProperty("allMonsters");

        database = target as MonsterDatabase;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Generate"))
        {
            EditorHelperFunctions.GenerateFromAsset("/Prefabs/Monsters", ref database.allMonsters, target, "*.prefab");
        }
        EditorGUILayout.PropertyField(databaseProperty, true);

        serializedObject.ApplyModifiedProperties();
    }
}