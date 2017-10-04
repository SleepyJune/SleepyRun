using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneChanger
{
    public static StageInfo currentStageInfo;

    public static void ChangeScene(string str)
    {
        SceneManager.LoadScene(str);       
    }
}
