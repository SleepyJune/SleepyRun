using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public static class LayerConstants
{
    public static int playerLayer = LayerMask.NameToLayer("Player");
    public static int monsterLayer = LayerMask.NameToLayer("Monsters");
    public static int wallLayer = LayerMask.NameToLayer("Walls");
    public static int groundLayer = LayerMask.NameToLayer("Ground");

    public static int pickupLayer = LayerMask.NameToLayer("Pickups");

    public static int monsterSpellLayer = LayerMask.NameToLayer("MonsterSpell");
    public static int playerSpellLayer = LayerMask.NameToLayer("PlayerSpell");

    public static int monsterMask = LayerMask.GetMask("Monsters");
    public static int playerMask = LayerMask.GetMask("Player");

    public static int clickableMask = LayerMask.GetMask("Pickups");

    //public static int playerAndMonsterMask = LayerMask.GetMask("Player", "Monsters");

}
