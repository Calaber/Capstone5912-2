using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public int blueTeamScore;
    public int redTeamScore;
    public int scoreToWin;

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(blueTeamScore);
            stream.SendNext(redTeamScore);
            stream.SendNext(scoreToWin);
        }
        else
        {
            blueTeamScore = (int)stream.ReceiveNext();
            redTeamScore  = (int)stream.ReceiveNext();
            scoreToWin    = (int)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void blueTeamScored()
    {
        blueTeamScore++;
        if (blueTeamScore > scoreToWin)
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
        if (redTeamScore > scoreToWin)
        {
            Debug.Log("Red wins. Handle it please.");
        }
        else
        {
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
