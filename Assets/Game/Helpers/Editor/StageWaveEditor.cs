using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageWave))]
public class StageWaveEditor : Editor
{
    StageWave stageWave;

    public StageInfoEditor parent;
    public int subEditorIndex;

    public bool showFoldout = true;
    
    SerializedProperty stageEventsProperty;

    StageEventDatabase stageEventDatabase;

    List<Editor> stageEventEditors = new List<Editor>();
    List<bool> showFoldouts = new List<bool>();

    float lineHeight = EditorGUIUtility.singleLineHeight;

    void OnEnable()
    {
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        stageWave = (StageWave)target;
        
        stageEventsProperty = serializedObject.FindProperty("stageEvents");

        var assetPath = "Assets/Prefabs/Stage/Event Database/StageEvent Database.asset";
        stageEventDatabase = AssetDatabase.LoadAssetAtPath(assetPath, typeof(StageEventDatabase)) as StageEventDatabase;

        foreach (var stageEvent in stageWave.stageEvents)
        {
            stageEventEditors.Add(CreateEditor(stageEvent));
            showFoldouts.Add(true);
        }
    }

    private void OnDisable()
    {
        //CleanupEditors();
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ExpandedGUI();

        serializedObject.ApplyModifiedProperties();
    }

    private void ExpandedGUI()
    {
        EditorGUILayout.Space();
        var startRect = GUILayoutUtility.GetLastRect();

        var guiLabelStyle = new GUIStyle();
        guiLabelStyle.fontStyle = FontStyle.Bold;

        EditorGUILayout.Space();

        for (int i = 0; i < stageWave.stageEvents.Length; i++)
        {
            var stageEvent = stageWave.stageEvents[i];

            var eventTitle = stageEvent.monster != null ? 
                stageEvent.monster.name
                : stageEvent.eventName;

            showFoldouts[i] = EditorGUILayout.Foldout(showFoldouts[i], eventTitle);

            if (showFoldouts[i])
            {
                var editor = stageEventEditors[i];
                var lastRect = GUILayoutUtility.GetLastRect();

                editor.OnInspectorGUI();
                CreateClickable(lastRect, i, true);
            }

            EditorGUILayout.Separator();
        }

        EditorGUILayout.Space();

        CreateClickable(startRect, -1, true);
    }

    void CreateClickable(Rect lastRect, int index, bool isCondition)
    {
        Rect currentRect = GUILayoutUtility.GetLastRect();
        Rect clickArea = lastRect;
        clickArea.height = Mathf.Max(lineHeight, currentRect.y - lastRect.y);

        Event current = Event.current;

        if (clickArea.Contains(current.mousePosition) && current.type == EventType.ContextClick)
        {
            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < stageEventDatabase.allEventOptions.Length; i++)
            {
                var optionIndex = i;
                var option = stageEventDatabase.allEventOptions[i];
                string name = "Add Event/" + option;
                menu.AddItem(new GUIContent(name), false, () => AddEvent(optionIndex));
            }

            if (index >= 0)
            {
                menu.AddItem(new GUIContent("Remove Event"), false, () => RemoveEvent(index));
            }
            
            menu.ShowAsContext();
            current.Use();
        }
    }

    void AddEvent(int selectedIndex)
    {
        var type = stageEventDatabase.allEvents[selectedIndex].GetType();

        var newEvent = CreateInstance(type) as StageEvent;
        newEvent.name = type.ToString();

        AssetDatabase.AddObjectToAsset(newEvent, target);
        stageEventsProperty.AddToObjectArray(newEvent);

        stageEventEditors.Add(CreateEditor(newEvent));
        showFoldouts.Add(true);
    }

    void RemoveEvent(int index)
    {
        var subasset = stageWave.stageEvents[index];
        stageEventsProperty.RemoveFromObjectArrayAt(index);
        DestroyImmediate(subasset, true);
        stageEventEditors.RemoveAt(index);
        showFoldouts.RemoveAt(index);
    }

    public void ShowSubFoldouts(bool show)
    {
        for (int i = 0; i < showFoldouts.Count; i++)
        {
            showFoldouts[i] = show;
        }
    }
}
