using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{

    [System.NonSerialized]
    public int comboCount = 0;

    public Transform comboTextHolder;

    public GameObject textPrefab;

    GameObject lastText;
    int lastNewCount = 0;

    float lastTextTime = 0;
    float updateFrequency = .25f;

    public void IncreaseComboCount()
    {
        comboCount += 1;

        var countDiff = comboCount - lastNewCount;

        //Debug.Log(countDiff * updateFrequency);

        int tCount = comboCount;
        DelayAction.Add(() => MakeComboText(tCount), countDiff * updateFrequency);
    }

    public void MakeComboText(int count)
    {
        if (lastText)
        {
            Destroy(lastText);
        }

        var newText = Instantiate(textPrefab, comboTextHolder);
        var countTransform = newText.transform.Find("Count");
        countTransform.GetComponent<Text>().text = "x" + count;

        lastText = newText;
        lastNewCount = count;
    }
}
