using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEffect : MonoBehaviour {

    public GameObject waterPanel;
    public AudioClip splash;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            waterPanel.SetActive(true);
            SoundManager.instance.PlaySound(splash, true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            waterPanel.SetActive(false);
            SoundManager.instance.PlaySound(splash, true);
        }
    }


}
