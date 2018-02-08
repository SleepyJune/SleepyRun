using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public TextScriptDatabase textScriptDatabase;

    Dictionary<string, TextScript> textScriptDictionary = new Dictionary<string, TextScript>();

    public Text bossText;
    
    public GameObject tutorialObject;
    public TutorialParentController tutorialParent;

    public Image monsterImage;

    Animator anim;
    
    void Start()
    {
        foreach (var textScript in textScriptDatabase.allTexts)
        {
            textScriptDictionary.Add(textScript.name, textScript);
        }

        anim = tutorialObject.GetComponent<Animator>();
    }

    public void ShowMonsterInfo(Monster monster)
    {
        monsterImage.sprite = monster.image;
        bossText.text = monster.description;
    }

    public void DisableTutorialOverlay()
    {
        tutorialParent.tutorialLock = false;
        GameManager.instance.ResumeGame(false);
        tutorialObject.SetActive(false);
    }

    public void SetBossText(string str)
    {
        TextScript textScript;

        if(textScriptDictionary.TryGetValue(str, out textScript))
        {
            if (textScript)
            {
                bossText.text = textScript.text;
            }
        }
    }

    public void NextDialogue()
    {
        anim.SetTrigger("Next");
    }
}
