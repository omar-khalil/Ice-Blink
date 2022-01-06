using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    public bool grabbed = false;

    public void Grab()
    {
        if (!grabbed)
        {
            transform.parent = ItemSpawner.instance.hand;
            transform.localPosition = Vector3.zero;
            grabbed = true;
        } else
        {
            transform.parent = null;
            grabbed = false;
        }
    }

    public void OnMouseDown()
    {
        Grab();
    }

}
