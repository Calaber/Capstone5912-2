using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour {

    //Get Damage flash Image
    public Image myPanel;
    static Image dmgPanel;
    //color of the image, Used to get and set the alpha value
    static Color flashColor;

    //Damage Flash should suddenly appear then fade out. The lower the value the slower the fade out.
    private float fadeRate;

    //Damage flash image alpha value going from max to no alpha should adjust the transparency
    private float alpha;


	// Use this for initialization
	void Start () {
        dmgPanel = myPanel;
        alpha = 0f;
        fadeRate = 2;
        flashColor = dmgPanel.color;
    }

    //Starts a Damage flash. Call this to initiate a damage flash	
    public static void DamageFlash()
    {
        //set image alpha to 1(opaque)
        flashColor.a = 1f;
        dmgPanel.color = flashColor;
        
    }

	// Update is called once per frame
	void Update () {
        //get image colors
        flashColor = myPanel.color;

        //find the alpha Differential
        float alphaDiff = Mathf.Abs(flashColor.a - alpha);
        //if there are alpha differences decrease the alpha.
        if (alphaDiff > 0.0001f)
        {
            flashColor.a = Mathf.Lerp(flashColor.a, alpha, fadeRate * Time.deltaTime);
            myPanel.color = flashColor;
        }
    }
}
