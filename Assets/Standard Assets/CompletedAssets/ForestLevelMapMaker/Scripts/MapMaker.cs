using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MapType { Vertical, Horizontal }
public class MapMaker : MonoBehaviour
{
    public List<GameObject> BackgroundPrefabs;
    public MapType mapType;
    public List<Biome> biomes;

    private List<LevelButton> levelButtons;
    private int currentLevel;
    private int topPassedLevel = -1;

    public GameObject levelSelectManager;
        
    private void OnEnable()
    {
        CleanExcessBiomes();
        CleanLostBiomes();
    }

    public bool HasEmptyPrefabs()
    {
        if (BackgroundPrefabs == null || BackgroundPrefabs.Count == 0) return true;
        foreach (var bp in BackgroundPrefabs)
        {
            if (bp == null) return true;
        }
        return false;
    }

    void Start()
    {
        levelButtons = new List<LevelButton>();
        biomes.ForEach((b) => { levelButtons.AddRange(b.levelButtons); });

        levelSelectManager = transform.parent.gameObject;

        string topLevelString = "TopLevelPassed";
        if (PlayerPrefs.HasKey(topLevelString))
        {
            topPassedLevel = PlayerPrefs.GetInt(topLevelString);
        }
        else
        {
            topPassedLevel = 1;
            //topPassedLevel = stageDatabase.databaseArray.Length - 2;
        }

        string lastLevelString = "LastLevelPlayed";
        if (PlayerPrefs.HasKey(lastLevelString))
        {
            currentLevel = PlayerPrefs.GetInt(lastLevelString);
        }
        else
        {
            currentLevel = topPassedLevel;
        }

        for (int i = 0; i < levelButtons.Count; i++)
        {
            int scene = i;


            /*levelButtons[i].button.onClick.AddListener(() =>
            {
            //set your own callbacks for buttons - for example
            Debug.Log("Press level button: " + scene);
                        
            });*/

            levelButtons[i].numberText.text = (scene + 1).ToString();
            
            SetButtonActive(scene, (currentLevel == scene || scene == topPassedLevel + 1), (topPassedLevel >= scene), scene);
        }
        
        Invoke("ScrollToElement", .25f); //need to wait a bit to get the height
    }

    void ScrollToElement()
    {
        GameObject button = GameObject.Find("LevelButton " + (currentLevel + 1));
        if (button)
        {
            transform.parent.GetComponent<ScrollRectCenterOnElement>().CenterOnItem(button.GetComponent<RectTransform>());
        }
    }

    public void AddBiome(GameObject prefab)
    {
        if (!prefab)
        {
            Debug.Log("No prefab");
            return;
        }
        if (biomes == null) biomes = new List<Biome>();
        biomes.Add(CreateBiome(prefab));
    }

    private Biome CreateBiome(GameObject prefab)
    {
        if (!prefab)
        {
            Debug.Log("No prefab");
            return null;
        }
        GameObject b = Instantiate(prefab);
        b.transform.localScale = transform.lossyScale;
        b.transform.SetParent(transform);
        b.transform.localPosition = Vector3.zero;
        Image i = b.GetComponent<Image>();
        if (i) i.SetNativeSize();
        return b.GetComponent<Biome>();
    }

    /// <summary>
    /// Remove from hierarchy biomes without references in biomes list 
    /// </summary>
    public void CleanExcessBiomes()
    {
        // if (biomes == null || biomes.Count == 0) return;
        Biome[] b = GetComponentsInChildren<Biome>();
        bool dflag = false;
        for (int i = 0; i < b.Length; i++)
        {
            Biome bi = b[i];
            if (biomes == null || biomes.IndexOf(bi) == -1)
            {
                DestroyImmediate(bi.gameObject);
                dflag = true;
            }
        }
        if (dflag)
        {
            GC.Collect();
        }
    }

    /// <summary>
    /// Remove empty biomes from list
    /// </summary>
    public void CleanLostBiomes()
    {
        if (biomes == null || biomes.Count == 0) return;
        int length = biomes.Count;
        for (int i = 0; i < length; i++)
        {
            if (i >= biomes.Count) break;
            if (!biomes[i])
            {
                biomes.Remove(biomes[i]);
            }
        }
    }

    /// <summary>
    /// Remove Biome from List and Destroy
    /// </summary>
    /// <param name="index"></param>
    public void RemoveBiome(int index)
    {
        if (biomes != null && index < biomes.Count && index >= 0)
        {
            Biome b = biomes[index];
            if (b)
            {
                biomes.Remove(b);
                DestroyImmediate(b.gameObject);
            }
        }
    }

    /// <summary>
    /// Remove All Biomes from List and Destroy
    /// </summary>
    /// <param name="index"></param>
    public void Clean()
    {
        if (biomes == null || biomes.Count == 0) return;
        int length = biomes.Count;
        for (int i = length - 1; i >= 0; i--)
        {
            if (i < biomes.Count)
            {
                Biome b = biomes[i];
                if (b)
                {
                    biomes.Remove(b);
                    DestroyImmediate(b.gameObject);
                }
            }
        }

    }

    /// <summary>
    /// Rebuld the same Map
    /// </summary>
    /// <param name="index"></param>
    public void ReBuild()
    {
        int length = biomes.Count;

        if (biomes == null || length == 0) return;
        int lengthp = BackgroundPrefabs.Count;
        if (BackgroundPrefabs == null || lengthp == 0) return;

        //save  biomes prefabs
        GameObject[] bPrefs = new GameObject[length];
        for (int i = 0; i < length; i++)
        {
            for (int pi = 0; pi < length; pi++)
            {

                if (biomes[i].name.Contains(BackgroundPrefabs[pi].name))
                {
                    bPrefs[i] = BackgroundPrefabs[pi];
                    break;
                }
            }
        }

        Clean();

        foreach (var item in bPrefs)
        {
            AddBiome(item);
        }

        ReArrangeBiomes();
    }

    /// <summary>
    /// Rearrange Biomes positions
    /// </summary>
    public void ReArrangeBiomes()
    {
        //   Debug.Log("rearrange");
        if (biomes == null || biomes.Count == 0) return;

        int length = biomes.Count;

        biomes[0].transform.localPosition = new Vector3(0, 0, 0);
        if (length > 1)
        {
            for (int i = 1; i < length; i++)
            {
                Biome b = biomes[i];
                Biome bPrev = biomes[i - 1];
                switch (mapType)
                {
                    case MapType.Vertical:
                        float dy = i * bPrev.BiomeSize.y / 2 + i * b.BiomeSize.y / 2;
                        b.transform.localPosition = new Vector3(0, dy, 0);
                        b.transform.SetAsFirstSibling();
                        break;
                    case MapType.Horizontal:
                        float dx = i * bPrev.BiomeSize.x / 2 + i * b.BiomeSize.x / 2;
                        b.transform.localPosition = new Vector3(dx, 0, 0);
                        break;
                }

            }
        }
    }

    /// <summary>
    /// Change biome type at index
    /// </summary>
    /// <param name="index"></param>
    /// <param name="bType"></param>
    public void ChangeBiome(int index, GameObject prefab)
    {
        Biome b = biomes[index];

        Biome newBiome = CreateBiome(prefab);
        if (newBiome)
        {
            biomes.Remove(b);
            DestroyImmediate(b.gameObject);
            biomes.Insert(index, newBiome);
        }
    }

    /// <summary>
    /// Set button interactable (or no) and show stars(or lock image)
    /// </summary>
    /// <param name="sceneNumber"></param>
    /// <param name="active"></param>
    /// <param name="isPassed"></param>
    private void SetButtonActive(int sceneNumber, bool active, bool isPassed, int buttonID)
    {
        string saveKey = sceneNumber.ToString() + "_stars_";

        if (!PlayerPrefs.HasKey(saveKey))
        {
            PlayerPrefs.SetInt(saveKey, 0);
        }
        int activeStarsCount = PlayerPrefs.GetInt(saveKey);

        levelButtons[sceneNumber].SetActive(sceneNumber, active, activeStarsCount, isPassed, buttonID, levelSelectManager);
    }

    public void SetControlActivity(bool activity)
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            levelButtons[i].button.interactable = activity;
        }
    }
}
