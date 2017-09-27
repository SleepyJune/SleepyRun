using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public Material skybox;
    public Light lightPrefab;

    void Awake()
    {
        RenderSettings.skybox = skybox;
    }
}
