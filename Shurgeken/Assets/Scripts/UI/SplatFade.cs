using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplatFade : MonoBehaviour {

    public Image firstSplat;
    public GameObject SecondSplat;
    public GameObject Splat1Sound;
    public GameObject Splat2Sound;
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
        Splat1Sound.SetActive(true);
        Color curColor = firstSplat.color;
        curColor.a = 1.0f;
        firstSplat.color = curColor;
        
        yield return new WaitForSeconds(.2f);
        SecondSplat.SetActive(true);
        Splat2Sound.SetActive(true);
    }
}
