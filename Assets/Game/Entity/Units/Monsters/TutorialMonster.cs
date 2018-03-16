using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class TutorialMonster : Monster
{
    bool tutorialCompleted = false;

    string slashTutorialString = "Tutorial_Slash";
    string slideLeftTutorialString = "Tutorial_SlideLeft";
    string slideRightTutorialString = "Tutorial_SlideRight";

    void Update()
    {
        if (!isDead)
        {
            base.UnitUpdate();

            TutorialFunction();

            if (speed != 0 && !isRooted && !isImmovable)
            {
                //var dir = new Vector3(0, 0, 1);
                transform.position -= Vector3.forward * speed * Time.deltaTime;

                if (highestSlowPercent > 0.05)
                {
                    var reverseBeltSpeed = GameManager.instance.floorManager.beltSpeed * highestSlowPercent;

                    transform.position += Vector3.forward * reverseBeltSpeed * Time.deltaTime;
                }

                if (anim)
                {
                    anim.SetFloat("speed", speed);
                }
            }

            RemoveOffStage();
        }

        OnUnitUpdateEvent();
    }

    public override void RemoveOffStage()
    {
        if (player.transform.position.z - transform.position.z > 0)
        {
            //MonsterRemoveOffStage();
            GameManager.instance.monsterManager.AddMissedMonsterCount(this);
            GameManager.instance.monsterManager.SetDead(this);
        }
    }

    public override void CollideWithPlayer()
    {
        if (!isDead)
        {
            if (monsterType == MonsterCollisionMask.Good)
            {
                GameManager.instance.comboManager.IncreaseComboCount();
                GameManager.instance.monsterManager.AddMonsterCollectedCount(this);
                if (buffOnHit)
                {
                    GameManager.instance.player.InitializeBuff(this, buffOnHit);
                }
            }

            GameManager.instance.monsterManager.SetDead(this);
        }
    }

    bool CheckTutorialCompleted(string str)
    {
        if (true)//!PlayerPrefs.HasKey(str))
        {
            PlayerPrefs.SetInt(str, 1);
            tutorialCompleted = true;
            return true;
        }
        else
        {
            tutorialCompleted = true;
            return false;
        }

        return false;
    }

    void TutorialFunction()
    {
        if (!tutorialCompleted)
        {
            if (transform.position.z - player.transform.position.z <= 20)
            {
                if (monsterType == MonsterCollisionMask.Good)
                {                    
                    CollectTutorial();
                }
                else if (monsterType == MonsterCollisionMask.Bad)
                {
                    SlashTutorial();
                }
            }
        }
    }

    void CollectTutorial()
    {
        if (player.transform.position.x - transform.position.x >= .75) //left
        {
            if (CheckTutorialCompleted(slideLeftTutorialString))
            {
                GameManager.instance.stageEventManager.tutorialController.ShowSlideTutorial(false);
            }
        }
        else if(player.transform.position.x - transform.position.x <= -.75) //right
        {
            if (CheckTutorialCompleted(slideRightTutorialString))
            {
                GameManager.instance.stageEventManager.tutorialController.ShowSlideTutorial(true);
            }
        }
        
    }

    void SlashTutorial()
    {
        if (CheckTutorialCompleted(slashTutorialString))
        {
            GameManager.instance.stageEventManager.tutorialController.ShowSlashTutorial(this);
        }
    }
}
