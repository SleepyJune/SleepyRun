using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

[CreateAssetMenu(menuName = "Database/TextScript Database")]
public class TextScriptDatabase : ScriptableObject
{
    public TextScript[] allTexts = new TextScript[0];
}
