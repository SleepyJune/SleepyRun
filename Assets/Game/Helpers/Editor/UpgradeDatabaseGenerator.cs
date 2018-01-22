using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

using UnityEditor;

using System.Linq;

[CustomEditor(typeof(UpgradeDatabase))]
public class UpgradeDatabaseGenerator : Editor
{
    SerializedProperty databaseProperty;

    UpgradeDatabase database;

    private void OnEnable()
    {
        databaseProperty = serializedObject.FindProperty("allUpgrades");

        database = target as UpgradeDatabase;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Generate"))
        {
            EditorHelperFunctions.GenerateFromAsset("/Prefabs/Upgrades", ref database.allUpgrades, target, "*.asset");
        }
        EditorGUILayout.PropertyField(databaseProperty, true);

        serializedObject.ApplyModifiedProperties();
    }
}