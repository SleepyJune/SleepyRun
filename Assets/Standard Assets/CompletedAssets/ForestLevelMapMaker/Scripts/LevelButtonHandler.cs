using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class LevelButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    ScrollRect scrollRect;

    void Start()
    {
        scrollRect = transform.parent.parent.parent.parent.GetComponent<ScrollRect>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        scrollRect.enabled = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        scrollRect.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        scrollRect.enabled = true;
    }
}
