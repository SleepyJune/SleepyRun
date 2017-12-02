using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(MonsterConditionDatabase))]
public class MonsterConditionDatabaseGenerator : Editor
{
    SerializedProperty allConditionsProperty;
    SerializedProperty allConditionOptions;

    SerializedProperty allActionsProperty;
    SerializedProperty allActionOptions;

    MonsterConditionDatabase conditionDatabase;
    
    private void OnEnable()
    {
        conditionDatabase = target as MonsterConditionDatabase;
        allConditionsProperty = serializedObject.FindProperty("allConditions");
        allConditionOptions = serializedObject.FindProperty("allConditionOptions");

        allActionsProperty = serializedObject.FindProperty("allActions");
        allActionOptions = serializedObject.FindProperty("allActionOptions");
    }

    public override void OnInspectorGUI()
    {
        //is prefab
        if (AssetDatabase.Contains(target))
        {
            EditorGUILayout.LabelField("Cannot edit prefab");
            return;
        }

        serializedObject.Update();

        if (GUILayout.Button("Generate"))
        {
            EditorHelperFunctions.Generate(ref conditionDatabase.allConditions, target);

            List<string> options = new List<string>();
            foreach(var condition in conditionDatabase.allConditions)
            {
                options.Add(condition.GetType().ToString().Replace("Monster",""));
            }
            conditionDatabase.allConditionOptions = options.ToArray();

            EditorHelperFunctions.Generate(ref conditionDatabase.allActions, target);

            options = new List<string>();
            foreach (var condition in conditionDatabase.allActions)
            {
                options.Add(condition.GetType().ToString().Replace("Monster", ""));
            }
            conditionDatabase.allActionOptions = options.ToArray();


            EditorUtility.SetDirty(target);
        }

        var selected = EditorGUILayout.Popup("AllConditions", 0, conditionDatabase.allConditionOptions);
        //EditorGUILayout.PropertyField(allConditionsProperty, true);

        EditorGUILayout.Popup("AllActions", 0, conditionDatabase.allActionOptions);
        //EditorGUILayout.PropertyField(allActionsProperty, true);

        serializedObject.ApplyModifiedProperties();
    }        
}