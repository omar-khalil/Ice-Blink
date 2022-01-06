using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilEater : Item {

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<OilSpill>())
        {
            OilSpill otherOillSpill = other.GetComponent<OilSpill>();
            otherOillSpill.StartShrinking();
        }
    }

}
