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
        InteractionText();
    }

    public void InteractionText()
    {
        textBox.SetActive(true);
        interaction.text = "Push O to open the door and start the game";
    }

    public void DeactivateActionText()
    {
        textBox.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DeactivateActionText();
        }
    }

}
