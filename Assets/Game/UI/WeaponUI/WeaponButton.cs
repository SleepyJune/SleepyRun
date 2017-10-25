using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WeaponButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{    
    public GameObject weaponItemTemplate;

    public Transform weaponList;

    public Image portraitImage;

    List<WeaponItem> weaponItems = new List<WeaponItem>();

    CanvasGroup weaponListCanvasGroup;

    Vector2 homePosition;

    //Image weaponImage;

    Weapon[] weapons;    

    bool isDragged;

    Vector2 swipeStartPos;

    bool pointerInButton = false;

    Lane destinationLane = Lane.mid;

    Transform playerPortraitTransform;

    Animator anim;

    ComboManager comboManager;
    WeaponManager weaponManager;

    void Start()
    {
        playerPortraitTransform = transform.parent;

        comboManager = GameManager.instance.comboManager;
        weaponManager = GameManager.instance.weaponManager;

        anim = playerPortraitTransform.GetComponent<Animator>();

        weapons = GameManager.instance.weaponManager.weapons;

        //weaponList = transform.parent.Find("WeaponList");
        //weaponImage = transform.Find("WeaponImage").GetComponent<Image>();
        //weaponImage.sprite = GameManager.instance.weaponManager.defaultWeapon.facePortrait;

        anim.runtimeAnimatorController = GameManager.instance.weaponManager.defaultWeapon.facePortraitAC;
        portraitImage.sprite = GameManager.instance.weaponManager.defaultWeapon.facePortrait;

        weaponListCanvasGroup = weaponList.GetComponent<CanvasGroup>();

        foreach (var weapon in weapons)
        {            
            var newWeapon = Instantiate(weaponItemTemplate, weaponList);
            newWeapon.transform.Find("Image").GetComponent<Image>().sprite = weapon.image;

            var newMenuItem = newWeapon.GetComponent<WeaponItem>();
            newMenuItem.weapon = weapon;
            newMenuItem.weaponButton = this;

            weaponItems.Add(newMenuItem);
        }

        homePosition = transform.position;

        GameManager.instance.touchInputManager.touchStart += HideWeaponItems;
    }

    public void HideWeaponItems(Touch touch)
    {
        weaponListCanvasGroup.alpha = 0;
        weaponListCanvasGroup.blocksRaycasts = false;
    }

    public void OnWeaponButtonClick()
    {
        if(weaponListCanvasGroup.alpha == 0)
        {
            weaponListCanvasGroup.alpha = 1;
            weaponListCanvasGroup.blocksRaycasts = true;
        }
        else
        {
            weaponListCanvasGroup.alpha = 0;
            weaponListCanvasGroup.blocksRaycasts = false;
        }        
    }

    public void OnWeaponSelect()
    {
        weaponListCanvasGroup.alpha = 0;
        weaponListCanvasGroup.blocksRaycasts = false;

        //weaponImage.sprite = GameManager.instance.weaponManager.currentWeapon.facePortrait;

        anim.runtimeAnimatorController = GameManager.instance.weaponManager.currentWeapon.facePortraitAC;
        portraitImage.sprite = GameManager.instance.weaponManager.currentWeapon.facePortrait;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        swipeStartPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var endPos = eventData.position;

        var delta = (endPos - swipeStartPos);

        var playerPosition = GameManager.instance.player.transform.position;

        if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
        {
            if (delta.x > 0)
            {
                if(playerPosition.x < 0)
                {
                    destinationLane = Lane.mid;
                }
                else if(playerPosition.x >= 0)
                {
                    destinationLane = Lane.right;
                }
            }
            else
            {
                if (playerPosition.x > 0)
                {
                    destinationLane = Lane.mid;
                }
                else if (playerPosition.x <= 0)
                {
                    destinationLane = Lane.left;
                }
            }

            GameManager.instance.player.destinationLane = destinationLane;
        }
        else
        {
            if (delta.y <= 0)
            {

                OnWeaponButtonClick();
            }
            else
            {
                if (true)//comboManager.charged)
                {
                    weaponManager.UseUltimate();
                }
            }
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }
}
