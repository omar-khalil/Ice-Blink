using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photonConnect : MonoBehaviour {

    public static photonConnect instance;

    public string versionName = "0.1";

    public GameObject view1;

    public photonButtons photonB;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        ConnectToPhoton();
    }


    public void ConnectToPhoton()
    {
        PhotonNetwork.ConnectUsingSettings(versionName);

        Debug.Log("Connecting to photon...");
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("We are connected to master");
        
    }

    private void OnJoinedLobby()
    {
  
        Debug.Log("On joined Lobby");


    }

    private void OnDisconnectedFromPhoton()
    {
        
        Debug.Log("Disconnected from services");
    }


    private void OnFailedToConnectToPhoton()
    {
        Debug.Log("No internet detected");
    }
}
