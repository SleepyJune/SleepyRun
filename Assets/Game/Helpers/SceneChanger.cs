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
        DelayAction.Reset(); //Destroy all pending actions when changing scene
        SceneManager.LoadScene(str);       
    }
}
