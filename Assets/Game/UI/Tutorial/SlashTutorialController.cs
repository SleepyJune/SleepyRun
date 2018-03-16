using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class SlashTutorialController : MonoBehaviour
{
    void Start()
    {
        GameManager.instance.PauseGame();
        GameManager.instance.isGamePaused = false;

        GameManager.instance.touchInputManager.touchStart += OnTouchResumeHandler;
    }
    
    public void OnTouchResumeHandler(Touch touch)
    {
        /*Debug.Log(touch.position);

        if (pointerInBox)
        {
            GameManager.instance.weaponManager.DisableWeapon(false);
            GameManager.instance.ResumeGame();
            GameManager.instance.touchInputManager.touchStart -= OnTouchResumeHandler;
            Destroy(gameObject);
        }
        else
        {
            GameManager.instance.weaponManager.DisableWeapon(true);
        }*/

        GameManager.instance.ResumeGame();
        GameManager.instance.touchInputManager.touchStart -= OnTouchResumeHandler;
        Destroy(gameObject);
    }
}
