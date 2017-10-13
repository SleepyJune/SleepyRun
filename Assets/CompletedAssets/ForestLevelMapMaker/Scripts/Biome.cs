using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class Biome : MonoBehaviour {
    [SerializeField]

    public GameObject levelButtonPrefab;
    public int count;
    public List<LevelButton> levelButtons;
    public bool snapButtonsToCurve = true;

    public Vector2 BiomeSize
    {
        get { return GetComponent<RectTransform>().sizeDelta; }
    }

    private void OnValidate()
    {
        if (count < 0) count = 0;
    }

    public void Create()
    {
        LevelButton[] existingButtons = GetComponentsInChildren<LevelButton>();
        foreach (var b in existingButtons)
        {
            DestroyImmediate(b.gameObject);
        }
        levelButtons = new List<LevelButton>();

        for (int i = 0; i < count; i++)
        {
            CreateButton();
        }
        SetButtonsPositionOnCurve();
    }

    private void CreateButton()
    {
        if (levelButtons == null) levelButtons = new List<LevelButton>();
        if (!levelButtonPrefab)
        {
            Debug.Log("Set level buttons prefab");
            return;
        }
        GameObject b = Instantiate(levelButtonPrefab);
        b.transform.localScale = transform.lossyScale;
        b.transform.SetParent(transform);
        levelButtons.Add(b.GetComponent<LevelButton>());
    }

    List<Vector3> pos;
    CatmulRommSpline_1 curve;
    public void SetButtonsPositionOnCurve()
    {
        if (snapButtonsToCurve)
        {
            if (!curve) curve = GetComponent<CatmulRommSpline_1>();
            if (!curve) return;

            pos = curve.GetPositions(levelButtons.Count);
            if (levelButtons.Count > 0)
            {
                for (int i = 0; i < levelButtons.Count; i++)
                {
                  if(levelButtons[i])  levelButtons[i].transform.position = pos[i];
                }
            }
        }
    }

}


#if UNITY_EDITOR
[CustomEditor(typeof(Biome))]
public class BiomeEditor : Editor
{
    private Biome biome;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        biome = target as Biome;
        if (biome.gameObject.activeInHierarchy)
        {
            if (GUILayout.Button("ReBuild LevelButtons"))
            {
                Undo.RecordObject(biome, "ReBuild  LevelButtons");
                EditorUtility.SetDirty(biome);
                biome.Create();
            }
        }
    }
}
#endif