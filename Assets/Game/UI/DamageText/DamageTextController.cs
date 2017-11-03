using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DamageTextType
{
    Physical,
    Recovery
}

public class DamageTextController : MonoBehaviour {

    public GameObject damageTextPrefab;
    public GameObject gainHealthTextPrefab;

    GameObject canvas;

	// Use this for initialization
	void Start () {
        canvas = GameObject.Find("Canvas/DamageTextHolder");
	}

    public void CreateDamageText(Entity unit, string text, DamageTextType textType)
    {
        if(textType == DamageTextType.Recovery)
        {
            CreateDamageText(unit, text, gainHealthTextPrefab);
        }
        else
        {
            CreateDamageText(unit, text, damageTextPrefab);
        }
    }

    public void CreateDamageText(Entity unit, string displayedText, GameObject textPrefab)
    {
        Vector3 screenLocation = Camera.main.WorldToScreenPoint(
            unit.transform.position +
                new Vector3(Random.Range(-.2f, .2f), .5f + Random.Range(-.2f, .2f), Random.Range(-.2f, .2f)));
                        
        var text = Instantiate(textPrefab);
        text.transform.SetParent(canvas.transform, false);
        text.transform.position = screenLocation;
        text.GetComponentInChildren<Text>().text = displayedText;

        var sizeRatio = unit.GetRelativeSizeRatio();
        text.transform.localScale = new Vector3(sizeRatio, sizeRatio, sizeRatio);

        Destroy(text, 2);
    }
}
