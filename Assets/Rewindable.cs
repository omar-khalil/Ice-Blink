using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Rewindable : MonoBehaviour {

    public abstract void Record();
    public abstract void RewindAt(int timeFrame);
}
