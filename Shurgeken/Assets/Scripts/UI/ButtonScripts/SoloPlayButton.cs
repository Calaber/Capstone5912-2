using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SoloPlayButton : MonoBehaviour {

    /// <summary>
    /// Button observed
    /// </summary>
    public Button yourButton;

    /// <summary>
    /// Gets the button and adds a listener to it
    /// </summary>
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    /// <summary>
    /// Go to Main Menu on click
    /// </summary>
    void TaskOnClick()
    {
        PlayerSettings.PlayerRoomName = "Single";
        PlayerSettings.PlayGame = NetworkManager.GameType.SINGLE;
        PlayerSettings.JoiningRoom = false;
        SceneManager.LoadScene("LoadingSoloPlay");
    }
}
