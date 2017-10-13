using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using UnityEngine;
using UnityEditor;

public static class EditorHelperFunctions
{
    public static void Generate<T>(string path, ref T[] collection, Object target)
    {
        List<T> newList = new List<T>();

        var files = Directory.GetFiles(Application.dataPath + path, "*.asset", SearchOption.AllDirectories);
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
}
