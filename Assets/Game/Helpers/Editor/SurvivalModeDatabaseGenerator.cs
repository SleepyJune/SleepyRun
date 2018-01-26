using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

using UnityEditor;

using System.Linq;

[CustomEditor(typeof(SurvivalModeDatabase))]
public class SurvivalModeDatabaseGenerator : Editor
{
    SerializedProperty databaseProperty;
    //SerializedProperty beltSpeedIncreaseProperty;

    SerializedProperty infoPrefabProperty;
    SerializedProperty clipProperty;

    private void OnEnable()
    {
        databaseProperty = serializedObject.FindProperty("databaseArray");

        infoPrefabProperty = serializedObject.FindProperty("survivalModeInfoPrefab");
        clipProperty = serializedObject.FindProperty("clip");
        //beltSpeedIncreaseProperty = serializedObject.FindProperty("beltSpeedIncreaseRate");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //EditorGUILayout.PropertyField(beltSpeedIncreaseProperty, true);
        EditorGUILayout.PropertyField(infoPrefabProperty, true);
        EditorGUILayout.PropertyField(clipProperty, true);


        if (GUILayout.Button("Generate"))
        {
            Generate();
        }
        EditorGUILayout.PropertyField(databaseProperty, true);

        serializedObject.ApplyModifiedProperties();
    }

    void DestroyAllSubAssets()
    {
        SurvivalModeDatabase database = target as SurvivalModeDatabase;

        var path = AssetDatabase.GetAssetPath(database);
        var data = AssetDatabase.LoadAllAssetsAtPath(path);

        foreach(var asset in data)
        {
            if(asset.name != database.name)
            {
                DestroyImmediate(asset, true);
            }
        }
    }

    List<SurvivalModeMonsterInfo> GetMonsterInfos(Transform databaseTransform)
    {
        List<SurvivalModeMonsterInfo> monsterInfos = new List<SurvivalModeMonsterInfo>();

        foreach(var child in databaseTransform)
        {

        }


        return monsterInfos;
    }
    
    StageEvent AddRandomSpawnEvent(SurvivalModeMonsterInfo monsterInfo)
    {
        var spawnEvent = CreateInstance<RandomIntervalSpawnMonsterEvent>() as RandomIntervalSpawnMonsterEvent;
        spawnEvent.name = "Spawn " + monsterInfo.monster.name;

        spawnEvent.monster = monsterInfo.monster;
        spawnEvent.spawnFrequency = monsterInfo.spawnFrequency;

        if (monsterInfo.monster.isImmovable)
        {
            spawnEvent.zSpawnDistance = 30;
        }

        AssetDatabase.AddObjectToAsset(spawnEvent, target);

        return spawnEvent as StageEvent;
    }

    StageEvent AddBeltSpeedEvent(float speed)
    {
        var stageEvent = CreateInstance<StageBeltSpeedChangeEvent>() as StageBeltSpeedChangeEvent;
        stageEvent.name = "BeltSpeed";

        stageEvent.speed = speed;

        AssetDatabase.AddObjectToAsset(stageEvent, target);

        return stageEvent as StageEvent;
    }

    StageEvent AddVictoryConditionEvent()
    {
        var stageEvent = CreateInstance<GameOverOnCountdown>() as GameOverOnCountdown;
        stageEvent.name = "Victory";

        stageEvent.victoryOnCountdown = true;

        AssetDatabase.AddObjectToAsset(stageEvent, target);

        return stageEvent as StageEvent;
    }

    void Generate()
    {
        SurvivalModeDatabase database = target as SurvivalModeDatabase;

        List<StageInfo> infoList = new List<StageInfo>();

        var info = database.survivalModeInfoPrefab;
        var baseInfoTransform = info.transform.GetChild(0);
        var baseInfoScript = info.transform.GetChild(0).GetComponent<SurvivalModeInfo>();

        if (info == null)
        {
            return;
        }

        DestroyAllSubAssets();

        var monsterInfos = baseInfoTransform.GetComponentsInChildren<SurvivalModeMonsterInfo>();

        var clip = database.clip;

        for(int i = 0; i < clip.length * 60; i++)
        {
            if(i == 0)
            {
                continue;//skip 0th stage
            }

            clip.SampleAnimation(info, i/60.0f);

            //Debug.Log(baseInfoScript.beltSpeed);

            var stageInfo = CreateInstance<StageInfo>();
            var stageName = "Level " + i;

            stageInfo.stageId = i;
            stageInfo.stageName = stageName;
            stageInfo.name = stageInfo.stageName;
            AssetDatabase.AddObjectToAsset(stageInfo, target);
                        
            var stageWave = CreateInstance<StageWave>();
            stageWave.name = stageName + " wave";
            AssetDatabase.AddObjectToAsset(stageWave, target);

            stageInfo.stageWaves = new StageWave[1] { stageWave };

            List<StageEvent> stageEvents = new List<StageEvent>();

            //stage events

            foreach(var monsterInfo in monsterInfos)
            {
                if(monsterInfo.spawnFrequency != 0)
                {
                    stageEvents.Add(AddRandomSpawnEvent(monsterInfo));
                }

            }

            stageEvents.Add(AddBeltSpeedEvent(baseInfoScript.beltSpeed));
            stageEvents.Add(AddVictoryConditionEvent());

            stageWave.stageEvents = stageEvents.ToArray();

            foreach(var stageEvent in stageEvents)
            {
                stageEvent.name = stageName + " " + stageEvent.name;
            }

            infoList.Add(stageInfo);
        }        

        database.databaseArray = infoList.ToArray();
        EditorUtility.SetDirty(target);
    }
}