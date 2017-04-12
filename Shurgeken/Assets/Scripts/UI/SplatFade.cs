using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplatFade : MonoBehaviour {

    public Image firstSplat;
    public GameObject SecondSplat;
	// Use this for initialization
	void Start () {
        StartCoroutine(Splat());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator Splat()
    {
        yield return new WaitForSeconds(2.0f);
        Color curColor = firstSplat.color;
        curColor.a = 1.0f;
        firstSplat.color = curColor;
        yield return new WaitForSeconds(.1f);
        SecondSplat.SetActive(true);
    }
}
