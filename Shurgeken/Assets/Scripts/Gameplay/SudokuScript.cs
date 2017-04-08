using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SudokuScript : MonoBehaviour {

    // Use this for initialization

    public GameObject[][] labels;
    public int[][] sudoku_grid;

	void Start() {
        /*Generate initial grid*/

	}
	// Update is called once per frame
	void Update () {
		
	}

    bool CheckWinState() {
        bool notwin = false;

        bool[] in_set = new bool[10];

        for (int set = 0; set < 10; set++) {
            in_set[set] = false;
        }

        for (int i = 0; i < 10; i++) {
            for (int j = 0; j < 10; j++) {
                in_set[sudoku_grid[i][j]] = true;
            }
            for (int set = 0; set < 10; set++)
            {
                if(in_set[set] == false){ notwin = true; }
            }
        }

        for (int set = 0; set < 10; set++)
        {
            in_set[set] = false;
        }

        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 10; i++)
            {
                in_set[sudoku_grid[i][j]] = true;
            }
            for (int set = 0; set < 10; set++)
            {
                if (in_set[set] == false) { notwin = true; }
            }
        }
        return !notwin;
    }




    
    void OnMouseDown() {
        Debug.Log("Ping!");


    }

}
