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
}
