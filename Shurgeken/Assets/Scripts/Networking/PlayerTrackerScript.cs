using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrackerScript : Photon.MonoBehaviour
{

    [SerializeField]    
    List<int> redPlayersInJail;

    [SerializeField]
    List<int> bluePlayersInJail;

    [SerializeField]
    List<int> notGettingOut;

    PhotonView photonView;

    void Start()
    {
        redPlayersInJail = new List<int>();
        bluePlayersInJail = new List<int>();
        notGettingOut = new List<int>();
        photonView = this.GetComponent<PhotonView>();
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        /* No-op We dont care about this */
    }

    [PunRPC]
    public void AddPlayerToJail(int viewId, string team)
    {
        switch (team)
        {
            case "red":
                redPlayersInJail.Add(viewId);
                if (NetworkManager.networkManager.getPlayerCount() == redPlayersInJail.Count)
                {//lose condition
                    GameObject.Find("UI Popup").transform.FindChild("Lose").GetComponent<PopupFadeout>().StartPopup();

                }
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
        bluePlayersInJail = new List<int>();
    }

    [PunRPC]
    public void respawnRedTeam() {
        redPlayersInJail.ForEach(player => respawnAtBase(player));
        redPlayersInJail = new List<int>();
    }

    [PunRPC]
    public void respawnAllJail()
    {
        respawnBlueTeam();
        respawnRedTeam();
    }

    public void respawnAtBase(int id)
    {
        PhotonView.Find(id).RPC("RespawnAtBase", PhotonTargets.All);
    }
}
