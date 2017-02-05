using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour {

    //Get Damage flash Image
    public Image myPanel;
    Color imageColor;

    //Damage Flash should suddenly appear then fade out
    private int fadeTime;
    private int totalFadeTime;
    private float fadeRate;

    //Damage flash image alpha value going from max to no alpha should adjust the transparency
    private float alpha;
    private float maxAlpha;


	// Use this for initialization
	void Start () {

        totalFadeTime = 200;
        fadeTime = 0;
        alpha = 0f;
        maxAlpha = 255f;
        fadeRate = 5;
        imageColor = myPanel.color;
    }
	
    public void DamageFlash()
    {
        fadeTime = totalFadeTime;

        imageColor.a = 1f;
        myPanel.color = imageColor;
        
    }

	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("Pressed");
            DamageFlash();
        }
        imageColor = myPanel.color;
        float alphaDiff = Mathf.Abs(imageColor.a - alpha);
        if (alphaDiff > 0.0001f)
        {
            imageColor.a = Mathf.Lerp(imageColor.a, alpha, fadeRate * Time.deltaTime);
            myPanel.color = imageColor;
            Debug.Log(myPanel.color);
        }
    }
}
