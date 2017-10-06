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

    public Slider energyBar;
    public Animation energyBarAnim;

    public Button ultButton;
    Animator ultButtonAC;

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

        energyBarAnim["EnergyBar"].speed = 0.0f;
        energyBarAnim.Play("EnergyBar");

        ultButtonAC = ultButton.transform.GetComponent<Animator>();
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

        var energy = Mathf.Lerp(energyBar.value, currentEnergy, 2 * Time.deltaTime);
        energyBar.value = energy;
        energyBarAnim["EnergyBar"].time = energy/100;

        if (!charged && currentEnergy == maxEnergy)
        {
            ultButtonAC.SetTrigger("Charged");
            charged = true;
        }

        if(charged && currentEnergy != maxEnergy)
        {
            ultButtonAC.SetTrigger("Charging");
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

        ultButtonAC.SetTrigger("ComboBreak");

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
