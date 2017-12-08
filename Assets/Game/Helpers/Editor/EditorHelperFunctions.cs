using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using UnityEngine;
using UnityEditor;

public static class EditorHelperFunctions
{
    public static void GenerateFromAsset<T>(string path, ref T[] collection, Object target, string assetType = "*.asset")
    {
        List<T> newList = new List<T>();

        var files = Directory.GetFiles(Application.dataPath + path, assetType, SearchOption.AllDirectories);
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

    public static void Generate<T>(ref T[] collection, Object target)
    {
        List<T> newList = new List<T>();

        var types = System.AppDomain.CurrentDomain.GetAllDerivedTypes(typeof(T));

        var newObject = new GameObject();
        newObject.name = typeof(T).ToString();

        foreach(var type in types)
        {
            var newComponent = newObject.AddComponent(type);
            newList.Add((T)((object)newComponent));
        }

        newObject.transform.parent = ((MonoBehaviour)target).transform;

        collection = newList.ToArray();
        EditorUtility.SetDirty(target);
    }    
}
