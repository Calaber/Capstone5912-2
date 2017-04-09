using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySlect : MonoBehaviour {

    public Dropdown diffcultyDrop;

    void Start ()
    {
        SetDropdownIndex(1);
        diffcultyDrop.onValueChanged.AddListener(delegate {
            diffValueChangedHandler(diffcultyDrop);
        });
    }

    void Destroy()
    {
        diffcultyDrop.onValueChanged.RemoveAllListeners();
    }

    private void diffValueChangedHandler(Dropdown target)
    {
        int val = target.value;
        if(val == 0)
        {
            PlayerSettings.difficulty = 0;
        }
        else if(val == 1)
        {
            PlayerSettings.difficulty = 1;
        }
        else if (val == 2)
        {
            PlayerSettings.difficulty = 2;
        }
        else if (val == 3)
        {
            PlayerSettings.difficulty = 3;
        }
    }

    public void SetDropdownIndex(int index)
    {
        diffcultyDrop.value = index;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
