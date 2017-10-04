using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public static class LayerConstants
{
    public static int monsterLayer = LayerMask.NameToLayer("Monsters");
    public static int wallLayer = LayerMask.NameToLayer("Walls");

    public static int monsterMask = LayerMask.GetMask("Monsters");
    public static int playerMask = LayerMask.GetMask("Player");

    //public static int playerAndMonsterMask = LayerMask.GetMask("Player", "Monsters");

}
