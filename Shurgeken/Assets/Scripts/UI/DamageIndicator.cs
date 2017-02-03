using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour {

    //Get Damage flash Image
    public Image myPanel;

    //Damage Flash should suddenly appear then fade out
    private int fadeTime;
    private int totalFadeTime;

    //Damage flash image alpha value going from max to no alpha should adjust the transparency
    private float alpha;
    private float maxAlpha;


	// Use this for initialization
	void Start () {

        totalFadeTime = 5;
        fadeTime = 0;
        alpha = 0f;
        maxAlpha = 1f;
	}
	
    public void DamageFlash()
    {
        fadeTime = totalFadeTime;
        alpha = maxAlpha;
    }

	// Update is called once per frame
	void Update () {
        if (fadeTime > 0)
        {
            alpha = maxAlpha * (float)(totalFadeTime / fadeTime);
            myPanel.GetComponent<CanvasRenderer>().SetAlpha(alpha);
        }
	}
}
