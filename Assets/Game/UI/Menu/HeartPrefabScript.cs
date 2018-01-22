using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class HeartPrefabScript : MonoBehaviour
{
    public Image prefabIcon;

    public Sprite shieldIcon;
    public Sprite heartIcon;
        
    bool isShield = false;
    
    public void SetIcon(bool useShieldIcon)
    {
        if (useShieldIcon)
        {
            if (!isShield)
            {
                prefabIcon.sprite = shieldIcon;
                isShield = true;
            }
        }
        else
        {
            if (isShield)
            {
                prefabIcon.sprite = heartIcon;
                isShield = false;
            }
        }        
    }
}
