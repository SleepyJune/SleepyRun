using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance = null;

    private int gold = 0;
    private const string goldString = "Gold";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);

            InitGold();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void InitGold()
    {        
        gold = PlayerPrefs.GetInt(goldString, 0);
    }

    public int GetGold()
    {
        return gold;
    }

    public void IncreaseGold(int amount)
    {
        gold += amount;
        PlayerPrefs.SetInt(goldString, gold);
    }

    public bool DecreaseGold(int amount)
    {
        if (gold > amount)
        {
            gold -= amount;
            PlayerPrefs.SetInt(goldString, gold);
            return true;
        }

        return false;
    }
}
