using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrackerScript : Photon.MonoBehaviour
{

    List<int> redPlayersInJail;

    List<int> bluePlayersInJail;

    List<int> notGettingOut;

    PhotonView photonView;

    void Start()
    {
        redPlayersInJail = new List<int>();
        bluePlayersInJail = new List<int>();
        photonView = this.GetComponent<PhotonView>();
    }

    [PunRPC]
    public void AddPlayerToJail(int viewId, string team)
    {
        switch (team)
        {
            case "red":
                redPlayersInJail.Add(viewId);
                break;
            case "blue":
                bluePlayersInJail.Add(viewId);
                break;
            default:
                notGettingOut.Add(viewId);
                break;
        }
    }

    [PunRPC]
    public void respawnBlueTeam()
    {
        bluePlayersInJail.ForEach(player => respawnAtBase(player));
    }

    [PunRPC]
    public void respawnRedTeam() {
        redPlayersInJail.ForEach(player => respawnAtBase(player));
    }

    public void respawnAtBase(int id)
    {
        photonView.RPC("RespawnAtBase", PhotonPlayer.Find(id));
    }
}
