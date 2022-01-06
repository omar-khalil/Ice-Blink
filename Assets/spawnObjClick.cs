using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnObjClick : MonoBehaviour
{

    Ray myRay;
    RaycastHit hit;
    public GameObject objectToInstatiate;
    public bool canFlare;
    public GameObject flareCheck;

    // Update is called once per frame
    void Update()
    {
        myRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (canFlare && Physics.Raycast(myRay, out hit))
        {
            if (Input.GetMouseButtonDown(0))
                spawn();
        }

    }

    public void ToggleCanFlare()
    {
        canFlare = !canFlare;
        flareCheck.SetActive(canFlare);
    }

    private void spawn()
    {
        PhotonNetwork.Instantiate(objectToInstatiate.name, hit.point, Quaternion.identity, 0);
        objectToInstatiate.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
    }
}
