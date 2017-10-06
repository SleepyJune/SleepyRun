using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    public Slider energyBar;
    public Image fill;
    //public Animation energyBarAnim;

    float currentEnergy;
    float maxEnergy = 30;

    float staminaRegen = 20;

    Color greenColor = new Color(0, 1, 0, .5f);
    Color yellowColor = new Color(1, .92f, 0, .5f);

    void Start()
    {
        fill = energyBar.transform.Find("Fill Area/Fill").GetComponent<Image>();
    }

    void Update()
    {
        IncreaseStamina();

        var energy = 100 * currentEnergy / maxEnergy;// Mathf.Lerp(energyBar.value, currentEnergy, 2 * Time.deltaTime);
        energyBar.value = energy;
        //energyBarAnim["EnergyBar"].time = energy / 100;
    }

    void IncreaseStamina()
    {
        currentEnergy += staminaRegen * Time.deltaTime;

        currentEnergy = Math.Min(maxEnergy, currentEnergy);

        var weapon = GameManager.instance.weaponManager.currentWeapon;
        if (currentEnergy >= weapon.staminaCost)
        {
            fill.color = greenColor;
        }
    }

    public float DecreaseStamina(int energy)
    {
        var lastEnergy = currentEnergy;

        currentEnergy -= energy;
        currentEnergy = Math.Max(0, currentEnergy);

        var weapon = GameManager.instance.weaponManager.currentWeapon;
        if (currentEnergy < weapon.staminaCost)
        {
            fill.color = yellowColor;
        }

        return Math.Min(1, lastEnergy / weapon.staminaCost);
    }
}
