using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

public class ReviveWindow : CanvasGroupWindow
{
    public Text reviveCountText;

    public void SetReviveCount(int count)
    {
        reviveCountText.text = "x" + count;
    }
}
