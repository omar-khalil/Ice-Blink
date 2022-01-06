using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class photonHandler : MonoBehaviour {

    public photonButtons photonB;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
        DontDestroyOnLoad(this.transform);
    }

    public void createNewRoom()
    {
        PhotonNetwork.CreateRoom(photonB.createRoomInput, new RoomOptions() { MaxPlayers = 2 }, null);
    }

    public void joinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(photonB.joinRoomInput, roomOptions, TypedLobby.Default);
    }
    public void moveScene()
    {
        PhotonNetwork.LoadLevel("game");
    }

    private void OnJoinedRoom()
    {
        moveScene();
        Debug.Log("Connected to the room: \" " + photonB.joinRoomInput + " \"");
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        
    }

    
}  
