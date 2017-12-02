using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(StageEventDatabase))]
public class StageEventDatabaseGenerator : Editor
{
    SerializedProperty allEventsProperty;
    SerializedProperty allEventOptions;

    StageEventDatabase eventDatabase;

    private void OnEnable()
    {
        eventDatabase = target as StageEventDatabase;
        allEventsProperty = serializedObject.FindProperty("allEvents");
        allEventOptions = serializedObject.FindProperty("allEventOptions");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (GUILayout.Button("Generate"))
        {
            EditorHelperFunctions.GenerateFromAsset("/Prefabs/Stage/Event Database", ref eventDatabase.allEvents, target);

            List<string> options = new List<string>();
            foreach (var stageEvent in eventDatabase.allEvents)
            {
                options.Add(stageEvent.name);
            }
            eventDatabase.allEventOptions = options.ToArray();

            EditorUtility.SetDirty(target);
        }

        var selected = EditorGUILayout.Popup("All Events", 0, eventDatabase.allEventOptions);

        serializedObject.ApplyModifiedProperties();
    }    
}