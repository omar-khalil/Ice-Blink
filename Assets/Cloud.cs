using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : Rewindable
{

    public float speed;

    private List<CloudState> states;

    void Start()
    {
        states = new List<CloudState>();
        speed = speed * UnityEngine.Random.Range(0.9f, 1.1f);
    }

    void FixedUpdate()
    {
        if (!Rewinder.instance.rewinding && Rewinder.instance.currentTimeFrame <= Rewinder.instance.maxTimeFrame)
        {
            transform.Translate(transform.right * speed);
        }
    }

    public override void Record()
    {
        if (Rewinder.instance.currentTimeFrame < states.Count)
        {
            states.RemoveRange(Rewinder.instance.currentTimeFrame, states.Count - Rewinder.instance.currentTimeFrame);
        }
        states.Add(new CloudState(transform.position));
    }

    public override void RewindAt(int timeFrame)
    {
        CloudState state = states[timeFrame];
        transform.position = state.position;
    }
}

public class CloudState
{
    public Vector3 position;

    public CloudState(Vector3 position)
    {
        this.position = position;
    }
}
