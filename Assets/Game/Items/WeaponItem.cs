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
    public Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameManager.instance.weaponManager.SwitchWeapons(weapon);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(1, 1, 1, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color(1, 1, 1, .5f);
    }
}
