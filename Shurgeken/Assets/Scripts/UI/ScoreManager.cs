using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public Text redScore;
    public Text blueScore;
    private GameMaster gm;

	// Use this for initialization
	void Start () {
        gm = GameObject.FindObjectOfType<GameMaster>();

	}
	void CheckScore()
    {
        if (gm == null)
        {
            redScore.text = "0";
            blueScore.text = "0";
        }
        else
        {
            redScore.text = gm.redTeamScore.ToString();
            blueScore.text = gm.blueTeamScore.ToString();

        }

    }
	// Update is called once per frame
	void Update () {
        if (gm == null)
        {
            gm = GameObject.FindObjectOfType<GameMaster>();
        }
        CheckScore();
	}
}
