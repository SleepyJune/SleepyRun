using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(StageInfoDatabase))]
public class StageInfoListGenerator : Editor
{
    SerializedProperty databaseProperty;

    private void OnEnable()
    {
        databaseProperty = serializedObject.FindProperty("databaseArray");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Generate"))
        {
            Generate();
        }

        EditorGUILayout.PropertyField(databaseProperty, true);        

        serializedObject.ApplyModifiedProperties();
    }

    void Generate()
    {
        StageInfoDatabase database = target as StageInfoDatabase;

        List<StageInfo> stageList = new List<StageInfo>();

        string path = "/Prefabs/Stage/";

        var files = Directory.GetFiles(Application.dataPath + path, "*.asset", SearchOption.AllDirectories);
        foreach(var file in files)
        {
            string assetPath = "Assets" + file.Replace(Application.dataPath, "").Replace('\\', '/');
                        
            StageInfo stageInfo = AssetDatabase.LoadAssetAtPath(assetPath, typeof(StageInfo)) as StageInfo;
            if (stageInfo)
            {
                stageList.Add(stageInfo);                
            }
        }

        database.databaseArray = stageList.ToArray();
    }
}