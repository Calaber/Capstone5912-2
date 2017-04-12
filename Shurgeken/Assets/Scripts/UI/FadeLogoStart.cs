using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeLogoStart : MonoBehaviour {

    public float FadeRate;
    public Image logo;
    private float targetAlpha;
    float timer;
    bool loading;
    // Use this for initialization
    void Start () {
        targetAlpha = logo.color.a;
        timer = 0;
        loading = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (timer == 50)
        {
            FadeIn();
        }
        else
        {
            timer++;
        }
        Color curColor = logo.color;
        float alphaDiff = Mathf.Abs(curColor.a - targetAlpha);
        if (alphaDiff > 0.0001f)
        {
            curColor.a = Mathf.Lerp(curColor.a, targetAlpha, this.FadeRate * Time.deltaTime);
            logo.color = curColor;
        }
        if (logo.color.a >= .9998 && !loading)
        {
            SceneManager.LoadScene("MainMenu");
            loading = true;
        }
    }
    public void FadeOut()
    {
        targetAlpha = 0.0f;
    }

    public void FadeIn()
    {
        targetAlpha = 1.0f;
    }

}
