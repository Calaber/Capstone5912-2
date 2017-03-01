using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateLobbyList : MonoBehaviour {

    public GameObject buttonList;
    public Text currentPageNumber;
    public Text maxPageNumber;
    public Button Previous;
    public Button Next;

    RoomInfo[] roomArray;
    int currentPage;
    int maxPage;
    int pageListMax;

	// Use this for initialization
	void Start () {
        roomArray = NetworkManager.networkManager.getRoomList();
        currentPage = 1;
        maxPage = 1;
        pageListMax = buttonList.transform.childCount;
        currentPageNumber.text = currentPage.ToString();
        maxPageNumber.text = maxPage.ToString();

        Previous.interactable = false;
        Next.interactable = false;

        Button _Previous = Previous.GetComponent<Button>();
        _Previous.onClick.AddListener(PreviousPage);
        Button _Next = Next.GetComponent<Button>();
        _Next.onClick.AddListener(NextPage);
        RoomCheck();

    }


    void SetMaxPage()
    {
        roomArray = NetworkManager.networkManager.getRoomList();
        maxPage = roomArray.Length / 10;
        if (roomArray.Length % 10 > 0)
        {
            maxPage++;
            Previous.interactable = true;
            Next.interactable = true;
            if (currentPage == 1)
            {
                Previous.interactable = false;
            }
            if (currentPage == maxPage)
            {
                Next.interactable = false;
            }
        }
        else if (maxPage == 0)
        {
            maxPage = 1;
            Previous.interactable = false;
            Next.interactable = false;
        }

    }

    void GenerateList()
    {
        roomArray = NetworkManager.networkManager.getRoomList();
        for (int i = 0; i < pageListMax; i++)
        {
            string buttonName = buttonList.transform.GetChild(i).name;
            if (i+(10*(currentPage-1)) < roomArray.Length)
            {
                buttonList.transform.GetChild(i).GetComponent<Button>().interactable = true;
                buttonList.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = roomArray[i].Name;
                int maxP = roomArray[i].MaxPlayers;
                int curP = roomArray[i].PlayerCount;
                buttonList.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = curP.ToString() + " / " + maxP.ToString();
                Debug.Log(roomArray[i].Name);
            }
            else
            {

                buttonList.transform.GetChild(i).GetComponent<Button>().interactable = false;
                buttonList.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = "<Empty>";
                buttonList.transform.GetChild(i).GetChild(1).GetComponent<Text>().text = "0/0";
            }
        }
    }

    public void NextPage()
    {
        currentPage++;
        RoomCheck();
    }

    public void PreviousPage()
    {
        currentPage--;
        RoomCheck();
    }
    void RoomCheck()
    {
        SetMaxPage();
        GenerateList();

    }
    void Awake()
    {
        RoomCheck();
        Invoke("RoomCheck", 2);
        InvokeRepeating("RoomCheck", 0f, 10.0f);
    }
	// Update is called once per frame
	void Update () {


    }


}
