using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Events;

public class ExecuteScriptOnStart : MonoBehaviour
{
    public UnityEvent funcToCall = new UnityEvent();

    void Start()
    {
        funcToCall.Invoke();
    }
}
