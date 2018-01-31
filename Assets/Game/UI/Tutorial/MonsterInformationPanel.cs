using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class MonsterInformationPanel : MonoBehaviour
{
    public Image monsterImage;

    public Text monsterNameText;
    public Text monsterTypeText;
    public Text monsterDescText;

    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowMonsterInfo(Monster monster)
    {
        monsterImage.sprite = monster.image;

        monsterNameText.text = monster.displayName;
        monsterTypeText.text = monster.monsterType.ToString();
        monsterDescText.text = monster.description;

        TogglePanel(true);
    }

    public void TogglePanel(bool show = true)
    {
        if (show)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
