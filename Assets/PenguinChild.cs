using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinChild : MonoBehaviour {

    Penguin penguin;

	// Use this for initialization
	void Start () {
        penguin = GetComponentInParent<Penguin>();
	}
	
	public void ReturnParent()
    {
        penguin.Return();
    }
}
