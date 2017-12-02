using System.Collections.Generic;

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MonsterConditionCollection))]
public class MonsterConditionCollectionEditor : Editor
{
    MonsterConditionCollection conditionCollection;
    
    public MonsterActionInfoEditor parent;
    public int subEditorIndex;

    SerializedProperty descriptionProperty;
    SerializedProperty conditionsProperty;
    SerializedProperty actionCollectionProperty;
    SerializedProperty actionsProperty;

    MonsterConditionDatabase conditionDatabase;

    List<Editor> conditionEditors = new List<Editor>();
    List<Editor> actionEditors = new List<Editor>();

    float lineHeight = EditorGUIUtility.singleLineHeight;

    bool isPrefab = false;

    void OnEnable()
    {
        conditionCollection = (MonsterConditionCollection)target;

        descriptionProperty = serializedObject.FindProperty("description");
        conditionsProperty = serializedObject.FindProperty("conditions");

        actionCollectionProperty = serializedObject.FindProperty("actionCollection");
        SerializedObject propertyObj = new SerializedObject(actionCollectionProperty.objectReferenceValue);

        actionsProperty = propertyObj.FindProperty("actions");
        
        var assetPath = "Assets/Prefabs/Monsters/MonsterActions/ConditionDatabase.prefab";

        var databaseObject = AssetDatabase.LoadAssetAtPath(assetPath, (typeof(GameObject))) as GameObject;
        conditionDatabase = databaseObject.GetComponent<MonsterConditionDatabase>();

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

        isPrefab = AssetDatabase.Contains(target);
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

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        CreateClickable(startRect, -1, true);
    }
        
    void CreateClickable(Rect lastRect, int index, bool isCondition)
    {
        if (isPrefab)
        {
            return;
        }

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

        var newObject = new GameObject();
        var newCondition = newObject.AddComponent(type) as MonsterCondition;
        newCondition.name = type.ToString();
        newCondition.transform.parent = conditionCollection.transform;      

        //AssetDatabase.AddObjectToAsset(newCondition, target);
        conditionsProperty.AddToObjectArray(newCondition);

        conditionEditors.Add(CreateEditor(newCondition));
    }

    void RemoveCondition(int index)
    {
        var subasset = conditionCollection.conditions[index];
        conditionsProperty.RemoveFromObjectArrayAt(index);
        DestroyImmediate(subasset.gameObject, true);
        conditionEditors.RemoveAt(index);
    }

    void AddAction(int selectedIndex)
    {
        var type = conditionDatabase.allActions[selectedIndex].GetType();

        var newObject = new GameObject();
        var newAction = newObject.AddComponent(type) as MonsterAction;
        newAction.name = type.ToString();
        newAction.transform.parent = conditionCollection.transform;

        //AssetDatabase.AddObjectToAsset(newAction, target);
        actionsProperty.AddToObjectArray(newAction);

        actionEditors.Add(CreateEditor(newAction));
    }

    void RemoveAction(int index)
    {
        var subasset = conditionCollection.actionCollection.actions[index];
        actionsProperty.RemoveFromObjectArrayAt(index);
        DestroyImmediate(subasset.gameObject, true);
        actionEditors.RemoveAt(index);
    }

    public static MonsterConditionCollection CreateConditionCollection()
    {
        var newGameObject = new GameObject();
        var newConditionCollection = newGameObject.AddComponent<MonsterConditionCollection>();
        newConditionCollection.description = "New Move Set";

        var newActionCollection = newGameObject.AddComponent<MonsterActionCollection>();
        newConditionCollection.actionCollection = newActionCollection;

        return newConditionCollection;
    }
}
