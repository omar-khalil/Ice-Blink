using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour {

    public static ItemSpawner instance;

    public GameObject oilEater;
    public GameObject itemPanel;
    public Transform hand;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SpawnOilEater()
    {
        itemPanel.SetActive(false);
        GameObject oilEaterSpawn = Instantiate(oilEater, hand.transform.position, Quaternion.identity);
        oilEaterSpawn.GetComponent<Item>().Grab();
    }
}
