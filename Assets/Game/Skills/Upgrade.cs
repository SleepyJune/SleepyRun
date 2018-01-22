using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/New Upgrade")]
public class Upgrade : ScriptableObject
{
    public string upgradeName;
    public string description;

    public Sprite iconImage;

    public float baseStatValue = 0;
    public float maxStatValue = 1;
    public float valueIncreament = .1f;

    public int baseCost = 100;
    public int costIncreament = 50;
    public float costIncreamentExponential = 2f;

    public int maxLevel = 10;

    public UpgradeInfo[] stats;

    public enum TargetObject
    {
        Prefab,
        GameManager,
        Player,
        PickupCubeManager,
    }

    public TargetObject targetObj = TargetObject.Prefab;
    public Object prefabTargetObj;
    public string targetString;

    public void ApplyUpgrade(int level)
    {
        Object target;

        if(targetObj != TargetObject.Prefab)
        {
            if(targetObj == TargetObject.Player)
            {
                target = GameManager.instance.player;
            }
            else if (targetObj == TargetObject.PickupCubeManager)
            {
                target = GameManager.instance.spawnPickupManager;
            }
            else
            {
                target = GameManager.instance;
            }
        }
        else
        {
            target = prefabTargetObj;
        }

        if(level < stats.Length)
        {
            var stat = stats[level];

            target.GetType().GetField(targetString).SetValue(target, stat.value);

            Debug.Log(target.GetType().GetField(targetString).GetValue(target));
        }

    }
}
