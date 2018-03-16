using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

class SlideTutorialController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool isSlidingRight = true;
    //public PlayerMovementControl playerMovement;

    public Text text;
    public Animator anim;

    public GameObject parentObject;

    bool tutorialComplete = false;
    Lane destinationLane = Lane.mid;
    Vector2 swipeStartPos;

    bool dragStarted = false;

    void Start()
    {
        GameManager.instance.PauseGame();

        if (isSlidingRight)
        {
            text.text = "Swipe Right To Collect Apple";
            anim.SetTrigger("SlideRight");
        }
        else
        {
            text.text = "Swipe Left To Collect Apple";
            anim.SetTrigger("SlideLeft");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        swipeStartPos = eventData.position;

        dragStarted = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //transform.position = eventData.position;

        var endPos = eventData.position;
        var delta = (endPos - swipeStartPos);

        if (Math.Abs(delta.x) > 50)
        {
            if (GameManager.instance.isGameOver || GameManager.instance.isGamePaused)
            {
                return;
            }
            else
            {
                OnEndDrag(eventData);
            }

            dragStarted = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!dragStarted)
        {
            return;
        }

        var endPos = eventData.position;

        var delta = (endPos - swipeStartPos);

        var playerPosition = GameManager.instance.player.transform.position;

        if (Mathf.Abs(delta.x) >= Mathf.Abs(delta.y))
        {
            if (GameManager.instance.player.isConfused)
            {
                delta.x *= -1;
            }

            if (isSlidingRight && delta.x > 0)
            {
                if (playerPosition.x < 0)
                {
                    destinationLane = Lane.mid;
                }
                else if (playerPosition.x >= 0)
                {
                    destinationLane = Lane.right;
                }

                tutorialComplete = true;
            }
            else if(!isSlidingRight && delta.x < 0)
            {
                if (playerPosition.x > 0)
                {
                    destinationLane = Lane.mid;
                }
                else if (playerPosition.x <= 0)
                {
                    destinationLane = Lane.left;
                }

                tutorialComplete = true;
            }

            if (tutorialComplete)
            {
                GameManager.instance.player.destinationLane = destinationLane;
                ResumeGame();
            }
        }
    }

    public void ResumeGame()
    {
        GameManager.instance.ResumeGame();
        Destroy(parentObject);
    }
}
