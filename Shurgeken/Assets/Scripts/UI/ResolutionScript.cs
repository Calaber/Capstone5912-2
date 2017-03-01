using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResolutionScript : MonoBehaviour {

    public Dropdown scRes;
    private static List<string> resList;
    private Resolution[] availableRes;

	// Use this for initialization
	void Start () {
        availableRes = Screen.resolutions;
        resList=new List<string>();
        CreateListOfRes();
        scRes.AddOptions(resList);
        Selected();

    }
	

    void CreateListOfRes()
    {

        string listEntry;

        foreach (Resolution res in availableRes)
        {
            listEntry=res.width + "x" + res.height;
            resList.Insert(0, listEntry);

        }
    }

    public void Selected()
    {
        string comparator;
        comparator= Screen.width + "x" + Screen.height;
        int foundindex=resList.IndexOf(comparator);
        scRes.value = foundindex;
        scRes.RefreshShownValue();
    }

	// Update is called once per frame
	void Update () {
        scRes.RefreshShownValue();
    }
}
