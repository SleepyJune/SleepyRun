using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponItem : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [NonSerialized]
    public Weapon weapon;

    [NonSerialized]
    public Image background;

    void Awake()
    {
        background = GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameManager.instance.weaponManager.SwitchWeapons(weapon);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        background.color = new Color(1, 1, 1, .5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        background.color = new Color(1, 1, 1, .1f);
    }
}
