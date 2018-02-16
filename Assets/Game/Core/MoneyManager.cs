using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance = null;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void IncreaseGold(int amount)
    {

    }

    public void DecreaseGold(int amount)
    {

    }
}
