using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StageInfo))]
public class StageInfoEditor : EditorWithSubEditors<StageWaveEditor, StageWave>
{
    StageInfo stageInfo;

    SerializedProperty stageIdProperty;
    SerializedProperty stageNameProperty;

    SerializedProperty stageWavesProperty;
    
    List<bool> showFoldouts = new List<bool>();

    float lineHeight = EditorGUIUtility.singleLineHeight;

    void OnEnable()
    {
        if (target == null)
        {
            DestroyImmediate(this);
            return;
        }

        stageInfo = (StageInfo)target;

        stageIdProperty = serializedObject.FindProperty("stageId");
        stageNameProperty = serializedObject.FindProperty("stageName");

        stageWavesProperty = serializedObject.FindProperty("stageWaves");

        CheckAndCreateSubEditors(stageInfo.stageWaves);

        foreach (var stageWave in stageInfo.stageWaves)
        {
            //stageWaveEditors.Add(CreateEditor(stageWave));
            showFoldouts.Add(true);
        }

        ShowStageEvents(false);
    }

    protected override void SubEditorSetup(StageWaveEditor editor, int index)
    {
        editor.parent = this;
        editor.subEditorIndex = index;
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
        CheckAndCreateSubEditors(stageInfo.stageWaves);

        EditorGUILayout.Space();
        var startRect = GUILayoutUtility.GetLastRect();

        var guiLabelStyle = new GUIStyle();
        guiLabelStyle.fontStyle = FontStyle.Bold;
        
        EditorGUILayout.PropertyField(stageIdProperty);
        EditorGUILayout.PropertyField(stageNameProperty);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Waves", guiLabelStyle);
                
        for (int i = 0; i < subEditors.Length; i++)
        {
            var stageWave = stageInfo.stageWaves[i];

            subEditors[i].showFoldout = EditorGUILayout.Foldout(subEditors[i].showFoldout, "Wave " + (i+1));

            var lastRect = GUILayoutUtility.GetLastRect();

            if (subEditors[i].showFoldout)
            {
                EditorGUI.indentLevel++;

                subEditors[i].OnInspectorGUI();
                EditorGUILayout.Space();

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Separator();

            
            CreateClickable(lastRect, i, true);
        }

        EditorGUILayout.Space();
                
        CreateClickable(startRect, -1, true);

        if (GUILayout.Button("Add New Wave", GUILayout.Width(150f)))
        {
            AddWave();
        }

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Show Less"))
        {
            ShowStageEvents(false);
        }

        if (GUILayout.Button("Show All"))
        {
            ShowStageEvents(true);
        }

        EditorGUILayout.EndHorizontal();

        /*if (GUILayout.Button("Convert Stage"))
        {
            ConvertStage();
        }*/
    }

    void ShowStageEvents(bool show)
    {
        for (int i = 0; i < subEditors.Length; i++)
        {
            var stageWave = stageInfo.stageWaves[i];

            subEditors[i].ShowSubFoldouts(show);
        }
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

            menu.AddItem(new GUIContent("Add New Wave"), false, () => AddWave());

            if (index >= 0)
            {
                menu.AddItem(new GUIContent("Remove Wave"), false, () => RemoveWave(index));
            }

            menu.ShowAsContext();
            current.Use();
        }
    }

    StageWave AddWave()
    {        
        var newEvent = CreateInstance(typeof(StageWave)) as StageWave;
        newEvent.name = "Wave " + (stageInfo.stageWaves.Length + 1);

        AssetDatabase.AddObjectToAsset(newEvent, target);
        stageWavesProperty.AddToObjectArray(newEvent);
        
        showFoldouts.Add(true);

        return newEvent;
    }

    void RemoveWave(int index)
    {        
        var subasset = stageInfo.stageWaves[index];
        stageWavesProperty.RemoveFromObjectArrayAt(index);

        foreach (var events in subasset.stageEvents)
        {
            DestroyImmediate(events, true);
        }

        DestroyImmediate(subasset, true);
        showFoldouts.RemoveAt(index);
    }

    void ConvertStage()
    {
        if(stageInfo.stageWaves.Length == 0)
        {
            var newWave = AddWave();

            var assetPath = AssetDatabase.GetAssetPath(target);
            var assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);

            foreach(var asset in assets)
            {
                if (AssetDatabase.IsSubAsset(asset) && asset is StageEvent)
                {
                    SerializedObject propertyObj = new SerializedObject(newWave);

                    var stageEventsProperty = propertyObj.FindProperty("stageEvents");
                    stageEventsProperty.AddToObjectArray(asset);
                }
            }
            
        }
    }
}
