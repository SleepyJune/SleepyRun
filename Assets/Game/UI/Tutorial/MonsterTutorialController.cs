using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MonsterTutorialController : MonoBehaviour
{   
    public void DestroyAndResumeGame()
    {
        GameManager.instance.ResumeGame();
        Destroy(gameObject);
    }
}
