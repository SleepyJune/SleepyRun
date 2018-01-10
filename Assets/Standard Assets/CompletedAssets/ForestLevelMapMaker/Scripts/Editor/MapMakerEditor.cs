using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(MapMaker))]
public class MapMakerEditor : Editor {

    private ReorderableList list;
    private MapMaker mapMaker;
    private List<SerializedObject> biomesSO;
    private List<SerializedObject> prefList;

    private void OnEnable()
    {
        if(Selection.activeObject) mapMaker = Selection.activeGameObject.GetComponent<MapMaker>();
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("biomes"), true, true, true, true);

        list.onRemoveCallback += RemoveCallback;
        list.drawElementCallback += OnDrawCallback;
        list.onAddCallback += OnAddCallBack;
        list.onSelectCallback += OnSelectCallBack;
        list.drawHeaderCallback += DrawHeaderCallBack;
        list.onChangedCallback += OnChangeCallBack;
        list.onAddDropdownCallback += OnAddDropDownCallBack;

        biomesSO = new List<SerializedObject>();
    }

    private void OnDisable()
    {
        if (list != null)
        {
            list.onRemoveCallback -= RemoveCallback;
            list.drawElementCallback -= OnDrawCallback;
            list.onAddCallback -= OnAddCallBack;
            list.onSelectCallback -= OnSelectCallBack;
            list.drawHeaderCallback -= DrawHeaderCallBack;
            list.onChangedCallback -= OnChangeCallBack;
            list.onAddDropdownCallback -= OnAddDropDownCallBack;
        }
    }

    private void OnChangeCallBack(ReorderableList list)
    {
        mapMaker.ReArrangeBiomes();// Debug.Log("onchange");
    }

    private void OnAddCallBack(ReorderableList list)
    {
        Debug.Log("OnAddCallBack");
    }

    private void OnAddDropDownCallBack(Rect buttonRect, ReorderableList list)
    {
        var menu = new GenericMenu();
        prefList = new List<SerializedObject>();
        if (mapMaker.BackgroundPrefabs != null && mapMaker.BackgroundPrefabs.Count > 0)
        {
            for (int i = 0; i < mapMaker.BackgroundPrefabs.Count; i++)
            {
                prefList.Add(new SerializedObject(mapMaker.BackgroundPrefabs[i]));
            }
        }

        for (int i = 0; i < prefList.Count; i++)
        {
            UnityEngine.Object g = prefList[i].targetObject;
            menu.AddItem(new GUIContent(i.ToString()+". "+g.name), false, ClickHandler, g);
        }

        menu.ShowAsContext();
    }

    private void ClickHandler(object obj)
    {
        Debug.Log("Selected: " + obj);
        mapMaker.AddBiome((GameObject) obj);//ReorderableList.defaultBehaviours.DoAddButton(list);
        biomesSO = new List<SerializedObject>();
        if (mapMaker.biomes != null)
        {
            for (int i = 0; i < mapMaker.biomes.Count; i++)
            {
                biomesSO.Add(new SerializedObject(mapMaker.biomes[i]));
            }
            mapMaker.ReArrangeBiomes();
        }

    }

    private void RemoveCallback(ReorderableList list)
    {
        if (EditorUtility.DisplayDialog("Warning!", "Are you sure?", "Yes", "No"))
        {
            mapMaker.RemoveBiome(list.index); //ReorderableList.defaultBehaviours.DoRemoveButton(list);
        }
        biomesSO = new List<SerializedObject>();
        for (int i = 0; i < mapMaker.biomes.Count; i++)
        {
            biomesSO.Add(new SerializedObject(mapMaker.biomes[i]));
        }
    }

    public override void OnInspectorGUI()
    {
        // help field
        EditorGUILayout.LabelField(GetType().ToString(), EditorUIUtil.guiTitleStyle);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Simple click on \"+\" button to add new background or \"-\" button to remove. Drag the elements to rearrange backgrounds in Level Screens List ", EditorUIUtil.guiMessageStyle);
        EditorGUILayout.Space();

        serializedObject.Update();

        //EditorGUILayout.PropertyField(serializedObject.FindProperty("stageDatabase"), true);

        //prefabs box
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.Space();
        EditorGUI.indentLevel += 1;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("BackgroundPrefabs"), true);
        EditorGUI.indentLevel -= 1;
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
        
        // prefab error box
        if (mapMaker.HasEmptyPrefabs() )
        {
            EditorGUILayout.HelpBox("Set biomes prefabs list.", MessageType.Error);
        }

        // clean map and create SirializedObject for each Biome 
        mapMaker.CleanLostBiomes();
        mapMaker.CleanExcessBiomes();

        biomesSO = new List<SerializedObject>();
        if (mapMaker.biomes != null)
        {
            for (int i = 0; i < mapMaker.biomes.Count; i++)
            {
                biomesSO.Add(new SerializedObject(mapMaker.biomes[i]));
            }
        }

        for (int i = 0; i < biomesSO.Count; i++)
        {
            biomesSO[i].Update();
        }

        // create MapType property field
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.Space();
        var sProp = serializedObject.FindProperty("mapType");
        var guiContent = new GUIContent();
        guiContent.text = sProp.displayName;
        EditorGUILayout.PropertyField(sProp, guiContent);
        EditorGUILayout.LabelField("Change Map Type and press <ReArrange> button.");
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();

        // show reordable list of Biomes
        list.DoLayoutList();

        serializedObject.ApplyModifiedProperties();

        // show rearrange button 
        if (GUILayout.Button("ReArrange Map"))
        {
            mapMaker.ReArrangeBiomes();
        }

        // show clean button 
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Remove all screens from map.");
        if (GUILayout.Button("Clear Map"))
        {
            if (EditorUtility.DisplayDialog("Warning!", "The map will be cleared.", "Yes", "No"))
            {
                mapMaker.Clean();
            }
        }
        EditorGUILayout.EndVertical();

        // show clean button 
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Clear and Rebuild map.");
        if (GUILayout.Button("ReBuild Map"))
        {
            mapMaker.ReBuild();
        }
        EditorGUILayout.EndVertical();
    }

    private void OnDrawCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        if (biomesSO != null && biomesSO.Count > 0)
        {
            EditorGUI.DropShadowLabel(new Rect(rect.x, rect.y,300, EditorGUIUtility.singleLineHeight), biomesSO[index].targetObject.name,EditorUIUtil.guiReordStyle);
        }
    }

    private void OnSelectCallBack(ReorderableList list)
    {
        GameObject biome = mapMaker.biomes[list.index].gameObject;
        if (biome)
            EditorGUIUtility.PingObject(biome);//   Selection.activeGameObject = biome;
    }

    private void DrawHeaderCallBack(Rect rect)
    {
        EditorGUI.LabelField(rect, "Level Screens");
    }
}
