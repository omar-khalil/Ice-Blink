using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilSpill : Rewindable
{

    public float speed;
    private List<OilState> states;

    public enum OilSpillHealthState
    {
        Normal,
        Shrinking
    }
    public OilSpillHealthState currentHealthState = OilSpillHealthState.Normal;

    void Start()
    {
        states = new List<OilState>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Rewinder.instance.rewinding && Rewinder.instance.currentTimeFrame <= Rewinder.instance.maxTimeFrame)
        {
            if (currentHealthState == OilSpillHealthState.Normal)
            {
                transform.Translate(-transform.forward * speed); 
            } else if (currentHealthState == OilSpillHealthState.Shrinking)
            {
                transform.localScale = transform.localScale * 0.99f;
            }
        }
    }

    public void StartShrinking()
    {
        currentHealthState = OilSpillHealthState.Shrinking;
    }

    public override void Record()
    {
        if (Rewinder.instance.currentTimeFrame < states.Count)
        {
            states.RemoveRange(Rewinder.instance.currentTimeFrame, states.Count - Rewinder.instance.currentTimeFrame);
        }
        states.Add(new OilState(transform.position, transform.rotation, transform.localScale,
            currentHealthState));
    }

    public override void RewindAt(int timeFrame)
    {
        OilState state = states[timeFrame];
        transform.position = state.position;
        transform.rotation = state.rotation;
        transform.localScale = state.scale;
        currentHealthState = state.currentHealthState;
    }
}

public class OilState
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public OilSpill.OilSpillHealthState currentHealthState;

    public OilState(Vector3 position, Quaternion rotation, Vector3 scale,
        OilSpill.OilSpillHealthState currentHealthState)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
        this.currentHealthState = currentHealthState;
    }
}
