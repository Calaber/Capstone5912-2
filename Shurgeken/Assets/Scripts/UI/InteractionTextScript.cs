using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionTextScript : MonoBehaviour {


    public GameObject textBox;
    private Text interaction;

	// Use this for initialization
	void Start () {
		interaction = GetComponent<Text>();

    }

    public void InteractionText(string Button, string action)
    {
        textBox.SetActive(true);
        interaction.text = "Push " + Button + " to " + action;
    }

    public void DeactivateActionText()
    {
        textBox.SetActive(false);
    }

}
