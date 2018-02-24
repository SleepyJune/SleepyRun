using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class HelperFunctions : MonoBehaviour
{
    public void LoadScene(string str)
    {
        SceneChanger.ChangeScene(str);
    }

    public void GameManagerExecute(string functionName)
    {
        GameManager.instance.SendMessage(functionName);
    }

    public void AnimatorTrigger(string triggerName)
    {
        GetComponent<Animator>().SetTrigger(triggerName);
    }

    public void SelfDestructImmediate()
    {
        Destroy(gameObject);
    }
}
