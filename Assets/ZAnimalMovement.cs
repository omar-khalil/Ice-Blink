using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZAnimalMovement : Rewindable {

    public float speed;
    private float initialSpeed;

    public float timeBetweenChange;
    private Vector3 displacement;
    private float timeSinceLastChange;

    private List<AnimalState> states;

	// Use this for initialization
	void Start () {
        timeSinceLastChange = timeBetweenChange;
        initialSpeed = speed;
        states = new List<AnimalState>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!Rewinder.instance.rewinding && Rewinder.instance.currentTimeFrame <= Rewinder.instance.maxTimeFrame)
        {
            MoveAnimal();
        }
    }

    void MoveAnimal()
    {
        if (timeSinceLastChange >= timeBetweenChange)
        {
            displacement = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f, UnityEngine.Random.Range(-1f, 1f)).normalized * speed;
            if (displacement.magnitude > 0)
            {
                transform.forward = displacement;
            }
            timeSinceLastChange = 0f;
            timeBetweenChange = UnityEngine.Random.Range(0.5f, 1f);
            speed = speed == initialSpeed ? 0 : initialSpeed;
        }
        else
        {
            timeSinceLastChange += Time.deltaTime;
        }
        transform.position += displacement;
    }

    public override void Record()
    {
        if (Rewinder.instance.currentTimeFrame < states.Count)
        {
            states.RemoveRange(Rewinder.instance.currentTimeFrame, states.Count - Rewinder.instance.currentTimeFrame);
        }
        states.Add(new AnimalState(transform.position, transform.rotation));
    }

    public override void RewindAt(int timeFrame)
    {
        AnimalState state = states[timeFrame];
        transform.position = state.position;
        transform.rotation = state.rotation;
    }
}

public class AnimalState
{
    public Vector3 position;
    public Quaternion rotation;

    public AnimalState(Vector3 position, Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}
