using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour {

    //Get Damage flash Image
    public Image myPanel;
    //color of the image, Used to get and set the alpha value
    Color imageColor;

    //Damage Flash should suddenly appear then fade out. The lower the value the slower the fade out.
    private float fadeRate;

    //Damage flash image alpha value going from max to no alpha should adjust the transparency
    private float alpha;


	// Use this for initialization
	void Start () {

        alpha = 0f;
        fadeRate = 2;
        imageColor = myPanel.color;
    }

    //Starts a Damage flash. Call this to initiate a damage flash	
    public void DamageFlash()
    {
        //set image alpha to 1(opaque)
        imageColor.a = 1f;
        myPanel.color = imageColor;
        
    }

	// Update is called once per frame
	void Update () {

        //Debug Key
        if (Input.GetKeyDown(KeyCode.M))
        {
            DamageFlash();
        }
        //get image colors
        imageColor = myPanel.color;

        //find the alpha Differential
        float alphaDiff = Mathf.Abs(imageColor.a - alpha);
        //if there are alpha differences decrease the alpha.
        if (alphaDiff > 0.0001f)
        {
            imageColor.a = Mathf.Lerp(imageColor.a, alpha, fadeRate * Time.deltaTime);
            myPanel.color = imageColor;
        }
    }
}
