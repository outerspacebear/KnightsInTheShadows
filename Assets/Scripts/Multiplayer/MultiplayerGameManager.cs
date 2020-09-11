using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerGameManager : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        if(!PhotonNetwork.IsConnected)
        {
            Debug.LogError("Player not connected to the network!");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
