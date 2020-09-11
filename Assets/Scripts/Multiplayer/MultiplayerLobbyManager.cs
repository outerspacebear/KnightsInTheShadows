using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerLobbyManager : MonoBehaviourPunCallbacks
{
    public void RefreshAvailableGames()
    {
        Debug.Log("Checking for available games!");
        RefreshUI();
    }

    public void JoinAvailableGame()
    {
        RefreshAvailableGames();
        if(!isGameAvailable)
        {
            Debug.Log("No games available!");
            if(infoText)
            {
                infoText.text = "Sorry, no games available!";
            }
            return;
        }

        //Join a game
    }

    public void CreateGame()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 2 };
        PhotonNetwork.CreateRoom("", roomOptions);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.AutomaticallySyncScene = true;

        Debug.Log("Connected to Photon network!");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        if (infoText)
        {
            infoText.text = "Game created; waiting for an opponent!";
        }
        Debug.Log("Room created!");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel(3);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        RefreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RefreshUI()
    {
        Debug.Log("Refreshing multiplayer menu UI!");

        isGameAvailable = PhotonNetwork.CountOfRooms > 0 ? true : false;

        if (joinGameButton)
        {
            joinGameButton.interactable = isGameAvailable;
        }
        if(infoText)
        {
            infoText.text = isGameAvailable ? "Game available!" : "No game available!";
        }
    }

    [SerializeField]
    Button joinGameButton = null;
    bool isGameAvailable = false;

    [SerializeField]
    Text infoText = null;
}
