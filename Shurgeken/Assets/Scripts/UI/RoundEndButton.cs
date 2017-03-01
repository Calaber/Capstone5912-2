using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RoundEndButton : MonoBehaviour {

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        NetworkManager.networkManager.leaveRoom();
        SceneManager.LoadScene("MainMenu");
    }

    public void Activate(bool active) {
        this.gameObject.SetActive(active);
        if (active) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (!active)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }

}
