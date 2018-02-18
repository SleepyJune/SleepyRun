using System.Collections;
using System.Collections.Generic;

using System.IO;

using UnityEngine;

using UnityEditor;

using System.Linq;

[CustomEditor(typeof(Upgrade))]
public class UpgradeStatGenerator : Editor
{
    SerializedProperty databaseProperty;
    Upgrade database;

    private void OnEnable()
    {
        databaseProperty = serializedObject.FindProperty("stats");

        database = target as Upgrade;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        if (GUILayout.Button("Generate"))
        {
            Generate();
        }
        EditorGUILayout.PropertyField(databaseProperty, true);

        serializedObject.ApplyModifiedProperties();
    }

    void DestroyAllSubAssets()
    {
        var database = target as Upgrade;

        var path = AssetDatabase.GetAssetPath(database);
        var data = AssetDatabase.LoadAllAssetsAtPath(path);

        foreach (var asset in data)
        {
            if (asset.name != database.name)
            {
                DestroyImmediate(asset, true);
            }
        }
    }

    public int RoundDigits(int i)
    {
        int digits = (int)(Mathf.Floor(Mathf.Log10(i) + 1)/2.0f);
        var digitPower = (int)Mathf.Pow(10, digits);

        return ((int)Mathf.Round(i / digitPower)) * digitPower;
    }

    UpgradeInfo AddUpgradeInfo(int level)
    {
        var info = CreateInstance<UpgradeInfo>() as UpgradeInfo;

        info.name = "Level " + level;
        info.level = level;
        info.value = database.baseStatValue + level * database.valueIncreament;
            //Mathf.Min(database.maxStatValue, database.baseStatValue + level * database.valueIncreament);

        if (level == 0)
        {
            info.cost = 0;
        }
        else if(level == 1)
        {
            info.cost = database.baseCost;
        }
        else
        {
            info.cost = RoundDigits(Mathf.RoundToInt(
                database.baseCost
                    + Mathf.Pow(database.costIncreamentExponential, level-1) * database.costIncreament));
        }

        

        AssetDatabase.AddObjectToAsset(info, target);

        return info as UpgradeInfo;
    }

    void Generate()
    {
        var upgradeList = new List<UpgradeInfo>();

        DestroyAllSubAssets();
                
        for (int i = 0; i < database.maxLevel; i++)
        {
            var upgradeInfo = AddUpgradeInfo(i);

            upgradeList.Add(upgradeInfo);
        }

        database.stats = upgradeList.ToArray();
        EditorUtility.SetDirty(target);
    }
}