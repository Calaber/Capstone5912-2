using UnityEngine;

public class GameMaster : Photon.MonoBehaviour {

    public int blueTeamScore;
    public int redTeamScore;
    public int scoreToWin;
    public int round_reset_timer;


    void Start() {
        round_reset_timer = -1;
    }

    void Update() {
        if (round_reset_timer > 0) {
            round_reset_timer--;
            if (round_reset_timer == 0) {
                GameObject.Find("UI Popup").transform.FindChild("ExitButton").GetComponent<RoundEndButton>().Activate(true);
            }
        }

    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(blueTeamScore);
            stream.SendNext(redTeamScore);
            stream.SendNext(scoreToWin);
            stream.SendNext(round_reset_timer);
        }
        else
        {
            blueTeamScore = (int)stream.ReceiveNext();
            redTeamScore  = (int)stream.ReceiveNext();
            scoreToWin    = (int)stream.ReceiveNext();
            round_reset_timer = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void blueTeamScored()
    {
        blueTeamScore++;
        if (blueTeamScore >= scoreToWin)
        {
            Debug.Log("Blue wins. Handle it please.");
        }
        else
        {
            RespawnPlayersInJail();
        }
    }

    [PunRPC]
    public void redTeamScored()
    {
        redTeamScore++;
        if (redTeamScore >= scoreToWin)
        {
            //Debug.Log("Red wins. Handle it please.");

            GameObject.Find("UI Popup").transform.FindChild("Win").GetComponent<PopupFadeout>().StartPopup();
            round_reset_timer = 100;
                //(Adam) TODO: ^ Not this. This is a bad way to do this, and won't work over the network.
        }
        else
        {
            GameObject.Find("UI Popup").transform.FindChild("Score").GetComponent<PopupFadeout>().StartPopup();
            RespawnPlayersInJail();
        }
    }

    private void RespawnPlayersInJail()
    {
        GameInitScript gis = GameInitScript.gis;
        gis.playerTracker.RPC("respawnAllJail", PhotonTargets.All);
        gis.StartCoroutine("SpawnFlag");
    }

}
