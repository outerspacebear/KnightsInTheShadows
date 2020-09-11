using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class MultiplayerUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!infoText || !infoPanel)
        {
            Debug.LogError("No infoText or infoPanel attached to the multiplayer UI manager!");
            return;
        }
        infoText.text = opponentsTurnText;
        infoPanel.SetActive(false);

        TeamEvents.teamTurnStartedEvent.AddListener(OnTeamTurnStarted);
    }

    private void OnDestroy()
    {
        TeamEvents.teamTurnStartedEvent.RemoveListener(OnTeamTurnStarted);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTeamTurnStarted(TeamBase team)
    {
        PhotonView photonView = GetComponent<PhotonView>();
        photonView.RPC("UpdateUIOnTurnStart", RpcTarget.All, team.GetTeamNumber());
    }

    [PunRPC]
    void UpdateUIOnTurnStart(int teamNumber)
    {
        if(PhotonNetwork.LocalPlayer.ActorNumber != teamNumber)
        {
            infoPanel.SetActive(true);
        }
        else
        {
            infoPanel.SetActive(false);
        }
    }

    [SerializeField]
    GameObject infoPanel = null;
    [SerializeField]
    Text infoText = null;

    const string opponentsTurnText = "Waiting (Another Player's Turn)";
}
