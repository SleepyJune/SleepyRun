using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Events;

public class HotkeyManager : MonoBehaviour
{
    public UnityEvent escapeKeyEvent;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeKeyEvent.Invoke();
        }
    }

    public void ChangeScene(string str)
    {
        SceneChanger.ChangeScene(str);
    }
}
