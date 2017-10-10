using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterConditionCollection))]
public class MonsterConditionCollectionEditor : Editor//EditorWithSubEditors<MonsterConditionEditor, MonsterCondition>
{
    MonsterConditionCollection conditionCollection;
    
    public MonsterActionManagerEditor parent;
    public int subEditorIndex;

    SerializedProperty descriptionProperty;
    SerializedProperty conditionsProperty;
    SerializedProperty actionCollectionProperty;
    SerializedProperty actionsProperty;

    MonsterConditionDatabase conditionDatabase;

    List<Editor> conditionEditors = new List<Editor>();
    List<Editor> actionEditors = new List<Editor>();

    float lineHeight = EditorGUIUtility.singleLineHeight;

    void OnEnable()
    {
        conditionCollection = (MonsterConditionCollection)target;

        descriptionProperty = serializedObject.FindProperty("description");
        conditionsProperty = serializedObject.FindProperty("conditions");

        actionCollectionProperty = serializedObject.FindProperty("actionCollection");
        SerializedObject propertyObj = new SerializedObject(actionCollectionProperty.objectReferenceValue);

        actionsProperty = propertyObj.FindProperty("actions");
        
        var assetPath = "Assets/Prefabs/Monsters/MonsterActions/ConditionDatabase.asset";
        conditionDatabase = AssetDatabase.LoadAssetAtPath(assetPath, typeof(MonsterConditionDatabase)) as MonsterConditionDatabase;

        foreach (var condition in conditionCollection.conditions)
        {
            conditionEditors.Add(CreateEditor(condition));
        }

        foreach (var action in conditionCollection.actionCollection.actions)
        {
            actionEditors.Add(CreateEditor(action));
        }

        //conditionsProperty = serializedObject.FindProperty("conditions");
        //actionCollectionProperty = serializedObject.FindProperty("actionCollection");
    }

    private void OnDisable()
    {
        //CleanupEditors();
    }

    /*protected override void SubEditorSetup(MonsterConditionEditor editor, int index)
    {
        //editor.conditionsProperty = conditionsProperty;
    }*/

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        descriptionProperty.isExpanded = EditorGUILayout.Foldout(descriptionProperty.isExpanded, descriptionProperty.stringValue);

        if (descriptionProperty.isExpanded)
        {
            ExpandedGUI();
        }

        serializedObject.ApplyModifiedProperties();
    }



    private void ExpandedGUI()
    {
        var startRect = GUILayoutUtility.GetLastRect();

        var guiLabelStyle = new GUIStyle();
        guiLabelStyle.fontStyle = FontStyle.Bold;

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(descriptionProperty);       

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Conditions", guiLabelStyle);
              

        for (int i = 0; i < conditionEditors.Count; i++)
        {
            var editor = conditionEditors[i];

            var lastRect = GUILayoutUtility.GetLastRect();

            editor.OnInspectorGUI();
            CreateClickable(lastRect, i, true);

            EditorGUILayout.Separator();
        }
        
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Actions", guiLabelStyle);

        for (int i = 0; i < actionEditors.Count; i++)
        {
            var editor = actionEditors[i];

            var lastRect = GUILayoutUtility.GetLastRect();

            editor.OnInspectorGUI();
            CreateClickable(lastRect, i, false);

            EditorGUILayout.Separator();
        }

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
            //Do a thing, in this case a drop down menu

            GenericMenu menu = new GenericMenu();

            for (int i = 0; i < conditionDatabase.allConditionOptions.Length; i++)
            {
                var optionIndex = i;
                var option = conditionDatabase.allConditionOptions[i];
                string name = "Add Condition/" + option;
                menu.AddItem(new GUIContent(name), false, () => AddCondition(optionIndex));
            }

            for (int i = 0; i < conditionDatabase.allActionOptions.Length; i++)
            {
                var optionIndex = i;
                var option = conditionDatabase.allActionOptions[i];
                string name = "Add Action/" + option;
                menu.AddItem(new GUIContent(name), false, () => AddAction(optionIndex));
            }

            if (index >= 0)
            {
                if (isCondition)
                {
                    menu.AddItem(new GUIContent("Remove Condition"), false, () => RemoveCondition(index));
                }
                else
                {
                    menu.AddItem(new GUIContent("Remove Action"), false, () => RemoveAction(index));
                }
            }

            menu.AddItem(new GUIContent("Remove Collection/Confirm"), false, () => parent.RemoveFromCollection(subEditorIndex));

            menu.ShowAsContext();


            current.Use();
        }
    }

    void AddCondition(int selectedIndex)
    {
        var type = conditionDatabase.allConditions[selectedIndex].GetType();

        var newCondition = CreateInstance(type) as MonsterCondition;

        AssetDatabase.AddObjectToAsset(newCondition, target);
        conditionsProperty.AddToObjectArray(newCondition);

        conditionEditors.Add(CreateEditor(newCondition));
    }

    void RemoveCondition(int index)
    {
        conditionsProperty.RemoveFromObjectArrayAt(index);
        conditionEditors.RemoveAt(index);
    }

    void AddAction(int selectedIndex)
    {
        var type = conditionDatabase.allActions[selectedIndex].GetType();

        var newAction = CreateInstance(type) as MonsterAction;

        AssetDatabase.AddObjectToAsset(newAction, target);
        actionsProperty.AddToObjectArray(newAction);

        actionEditors.Add(CreateEditor(newAction));
    }

    void RemoveAction(int index)
    {
        actionsProperty.RemoveFromObjectArrayAt(index);
        actionEditors.RemoveAt(index);
    }

    public static MonsterConditionCollection CreateConditionCollection()
    {
        MonsterConditionCollection newConditionCollection = CreateInstance<MonsterConditionCollection>();

        newConditionCollection.description = "New Move Set";

        newConditionCollection.actionCollection = CreateInstance<MonsterActionCollection>();

        return newConditionCollection;
    }
}
