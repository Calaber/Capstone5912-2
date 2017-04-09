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
        if (stream.isWriting)
        {
            stream.SendNext(redPlayersInJail.Count + bluePlayersInJail.Count + notGettingOut.Count);
            redPlayersInJail.ForEach(id => stream.SendNext("Red-" + id.ToString()));
            bluePlayersInJail.ForEach(id => stream.SendNext("Blue-" + id.ToString()));
            notGettingOut.ForEach(id => stream.SendNext("Other-" + id.ToString()));
        }
        else
        {
            string recieved;
            string[] split;
            int viewId;
            int count = (int)stream.ReceiveNext();
            for (int i = 0; count > i; i++)
            {
                recieved = (string)stream.ReceiveNext();
                split = recieved.Split('-');
                switch (split[0])
                {
                    case "Red":
                        viewId = int.Parse(split[1]);
                        if (!redPlayersInJail.Contains(viewId))
                        {
                            redPlayersInJail.Add(viewId);
                        }
                        break;
                    case "Blue":
                        viewId = int.Parse(split[1]);
                        if (!bluePlayersInJail.Contains(viewId))
                        {
                            bluePlayersInJail.Add(viewId);
                        }
                        break;
                    case "Other":
                        viewId = int.Parse(split[1]);
                        if (!notGettingOut.Contains(viewId))
                        {
                            notGettingOut.Add(viewId);
                        }
                        break;
                }
            }
        }
    }

    [PunRPC]
    public void AddPlayerToJail(int viewId, string team)
    {
        switch (team.ToLower())
        {
            case "red":
                if (!redPlayersInJail.Contains(viewId))
                {
                    redPlayersInJail.Add(viewId);
                    if (NetworkManager.networkManager.getPlayerCount() == redPlayersInJail.Count)
                    {//lose condition
                        GameObject.Find("UI Popup").transform.FindChild("Lose").GetComponent<PopupFadeout>().StartPopup();
                        GameMaster.gm.round_reset_timer = 100;
                    }
                }
                break;
            case "blue":
                if (!bluePlayersInJail.Contains(viewId))
                {
                    bluePlayersInJail.Add(viewId);
                }
                break;
            default:
                if (!notGettingOut.Contains(viewId))
                {
                    notGettingOut.Add(viewId);
                }
                break;
        }
    }

    [PunRPC]
    public void respawnBlueTeam()
    {
        bluePlayersInJail.ForEach(player => respawnAtBase(player));
        bluePlayersInJail.Clear();
    }

    [PunRPC]
    public void respawnRedTeam() {
        redPlayersInJail.ForEach(player => respawnAtBase(player));
        redPlayersInJail.Clear();
    }

    [PunRPC]
    public void respawnAllJail()
    {
        respawnBlueTeam();
        respawnRedTeam();
    }

    [PunRPC]
    public void removeFromJail(int id)
    {
        Debug.Log("Id removing: " + id);
        Debug.Log("The list: " + redPlayersInJail.ToString());
        Debug.Log("The count is: " + redPlayersInJail.Count);
        for (int i = 0; i < redPlayersInJail.Count; i++)
        {
            Debug.Log("The entry at i = " + i + " the entry: " + redPlayersInJail[i]);
            if (redPlayersInJail[i] == id)
            {
                redPlayersInJail.RemoveAt(i);
                break;
            }
        }
        Debug.Log("The list after: " + redPlayersInJail.ToString());
    }

    public void respawnAtBase(int id)
    {
        if (NetworkManager.networkManager.isMaster())
        {
            PhotonView.Find(id).RPC("RespawnAtBase", PhotonTargets.All, id);
        }
    }
}
