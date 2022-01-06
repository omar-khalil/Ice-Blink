using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class photonButtons : MonoBehaviour {

    public photonHandler pHandler;
    public AudioClip clickbutton;

    
    public string createRoomInput, joinRoomInput;

    public void onClickCreateRoom()
    {
        if (createRoomInput.Length >= 1)
            pHandler.createNewRoom();

    }
    public void onClickJoinRoom()
    {
        SoundManager.instance.PlaySound(clickbutton, false);
        pHandler.joinOrCreateRoom();
    }
	
}
