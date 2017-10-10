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
        serializedObject.Update();

        if (GUILayout.Button("Generate"))
        {
            Generate<MonsterCondition>("/Prefabs/Monsters/MonsterActions", ref conditionDatabase.allConditions, target);

            List<string> options = new List<string>();
            foreach(var condition in conditionDatabase.allConditions)
            {
                options.Add(condition.name);
            }
            conditionDatabase.allConditionOptions = options.ToArray();

            Generate<MonsterAction>("/Prefabs/Monsters/MonsterActions", ref conditionDatabase.allActions, target);

            options = new List<string>();
            foreach (var condition in conditionDatabase.allActions)
            {
                options.Add(condition.name);
            }
            conditionDatabase.allActionOptions = options.ToArray();


            EditorUtility.SetDirty(target);
        }

        var selected = EditorGUILayout.Popup("AllConditions", 0, conditionDatabase.allConditionOptions);
        EditorGUILayout.PropertyField(allConditionsProperty, true);

        EditorGUILayout.Popup("AllActions", 0, conditionDatabase.allActionOptions);
        EditorGUILayout.PropertyField(allActionsProperty, true);

        serializedObject.ApplyModifiedProperties();
    }

    void Generate<T>(string path, ref T[] collection, Object target)
    {
        List<T> newList = new List<T>();
        
        var files = Directory.GetFiles(Application.dataPath + path, "*.asset", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            string assetPath = "Assets" + file.Replace(Application.dataPath, "").Replace('\\', '/');
                        
            T asset = (T)(object)AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));
            if (asset != null)
            {                
                newList.Add(asset);
            }
        }

        collection = newList.ToArray();
        EditorUtility.SetDirty(target);
    }
}