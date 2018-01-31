using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class CanvasGroupWindow : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    protected bool isWindowOpen = false;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        HideWindow();
    }

    public virtual void ShowWindow()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        isWindowOpen = true;
    }

    public virtual void HideWindow()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        isWindowOpen = false;
    }

}
