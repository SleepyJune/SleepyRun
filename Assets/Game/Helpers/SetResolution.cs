using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class SetResolution : MonoBehaviour
{
    public static int currentQuality = -1;

    public static int nativeWidth = 1080;
    public static int nativeHeight = 1920;

    public enum ScreenQuality
    {
        High,
        Medium,
        Low,
    }
    
    public static string qualityKey = "ScreenQuality";

    public CanvasGroupWindow infoWindow;

    [NonSerialized]
    public static Vector2[] screenResolution = new Vector2[]
    {
        new Vector2(1080, 1920),
        new Vector2(720, 1280),
        new Vector2(270, 480),
    };

    public void InitResolution()
    {
        nativeHeight = Screen.height;
        nativeWidth = Screen.width;

        AdjustResolution();
    }

    public static void AdjustResolution()
    {
        var prefQuality = PlayerPrefs.GetInt(qualityKey, 0);

        if (prefQuality != currentQuality)
        {
            AdjustQuality(prefQuality);
        }
    }

    public void AdjustResolutionEx()
    {
        AdjustResolution();
    }

    public void SetPreferedQuality(bool change, int quality)
    {
        if (change)
        {            
            if (PlayerPrefs.GetInt(qualityKey, 0) != quality)
            {
                PlayerPrefs.SetInt(qualityKey, quality);

                if (infoWindow)
                {
                    infoWindow.ShowWindow();
                }
            }
        }
    }

    static void AdjustQuality(int quality = 0)
    {
        var resolution = screenResolution[quality];

        var height = resolution.y;
        var width = resolution.x;

        if(height > nativeHeight && width > nativeWidth)
        {
            Debug.Log("Screen size too small.");
            return;
        }

        float scale = (float)Screen.height / height;
        Screen.SetResolution(Mathf.RoundToInt(Screen.width / scale), Mathf.RoundToInt(Screen.height / scale), true);

        currentQuality = quality;

        PlayerPrefs.SetInt(qualityKey, quality);
    }
}
