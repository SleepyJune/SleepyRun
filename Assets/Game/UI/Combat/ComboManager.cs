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

    //public Slider energyBar;

    int maxEnergy = 100;
    int energyPerCombo = 10;
    int currentEnergy;
    
    GameObject lastText;
    int lastNewCount = 0;
    
    float updateFrequency = .25f;

    Queue<ComboInfo> comboQueue;

    [System.NonSerialized]
    public bool charged = false;

    float lastComboTime = 0;

    public UIUnitFrame_Bar manaBarScript;

    class ComboInfo
    {
        public int count;
        public float time;

        public ComboInfo(int count, float time)
        {
            this.count = count;
            this.time = time;
        }
    }

    void Awake()
    {
        comboQueue = new Queue<ComboInfo>();
    }

    void Start()
    {
        manaBarScript.SetMaxValue(maxEnergy);
    }

    void Update()
    {
        if(Time.time - lastComboTime > 2) //lose combo in 3 seconds
        {
            BreakCombo();
        }

        if (comboQueue.Count > 0)
        {
            var comboInfo = comboQueue.Peek();
            if (Time.time > comboInfo.time)
            {
                comboQueue.Dequeue();
                MakeComboText(comboInfo.count);
            }
        }

        //var energy = Mathf.Lerp(energyBar.value, currentEnergy, 2 * Time.deltaTime);
        //energyBar.value = energy;

        manaBarScript.SetValue(currentEnergy);

        if (!charged && currentEnergy == maxEnergy)
        {
            //ultButtonAC.SetTrigger("Charged");
            charged = true;
        }

        if(charged && currentEnergy != maxEnergy)
        {
            //ultButtonAC.SetTrigger("Charging");
            charged = false;
        }
    }

    public void IncreaseComboCount()
    {
        comboCount += 1;

        var countDiff = comboCount - lastNewCount;

        //Debug.Log(countDiff * updateFrequency);

        var newCombo = new ComboInfo(comboCount, Time.time + countDiff * updateFrequency);
        comboQueue.Enqueue(newCombo);

        currentEnergy += (int)Mathf.Round(energyPerCombo + comboCount * .2f);
        currentEnergy = Mathf.Min(maxEnergy, currentEnergy);

        lastComboTime = Time.time;
    }

    public void BreakCombo()
    {
        //currentEnergy = 0;
        comboCount = 0;
        
        comboQueue = new Queue<ComboInfo>();
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
