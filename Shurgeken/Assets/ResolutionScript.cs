using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResolutionScript : MonoBehaviour {

    public Dropdown scRes;
    private List<string> resList;
    private Resolution[] availableRes;

	// Use this for initialization
	void Start () {
        availableRes = Screen.resolutions;
        resList=new List<string>();
        CreateListOfRes();
        scRes.AddOptions(resList);
        Selected();
        int test = 0;
        test++;
        print(test);
    }
	

    void CreateListOfRes()
    {

        string listEntry;
        //There are 6 of each resolution in the array back to back
        int indexer = 0; 
        foreach (Resolution res in availableRes)
        {
            listEntry=res.width + "x" + res.height;
            if (indexer % 6 == 0)
            {
                resList.Insert(0,listEntry);
            }
            indexer++;
            

        }
    }

    void Selected()
    {
        string comparator = Screen.currentResolution.width + "x" + Screen.currentResolution.height;
        int foundindex=resList.IndexOf(comparator);
        scRes.value = foundindex;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
