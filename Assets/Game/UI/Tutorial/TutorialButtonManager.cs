using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class TutorialButtonManager : MonoBehaviour
{
    //public GameObject levelSelectManager;
    //public LevelButton levelButtonPrefab;
    
    public Button buttonPrefab;
    public Transform buttonHolder;

    public MonsterDatabase database;

    public MonsterInformationPanel infoPanel;

    public UnityEngine.UI.Extensions.HorizontalScrollSnap scrollSnapObject;

    Dictionary<string, Monster> monsterDatabase = new Dictionary<string, Monster>();

    void Start()
    {
        foreach (var monster in database.allMonsters)
        {
            if (monster != null && monster.image != null)
            {
                monsterDatabase.Add(monster.name, monster);

                var newButton = Instantiate(buttonPrefab);

                newButton.transform.SetParent(buttonHolder, false);
                newButton.onClick.AddListener(() =>
                {
                    ShowMonsterInformation(monster.name);
                });

                var image = newButton.GetComponent<Image>();
                image.sprite = monster.image;

                //newLevelButton.SetActive(monster.stageId, true, 3, false, levelSelectManager);
            }
        }

        scrollSnapObject.enabled = true;
    }

    void ShowMonsterInformation(string monsterName)
    {
        Monster monster;

        if(monsterDatabase.TryGetValue(monsterName, out monster))
        {
            infoPanel.ShowMonsterInfo(monster);
        }
    }

}
