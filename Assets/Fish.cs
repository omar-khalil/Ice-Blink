using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : Rewindable {

    public float speed;
    private List<FishState> states;

    public enum FishHealthState
    {
        Healthy,
        Toxic,
        Dead
    }
    public FishHealthState fishHealthState;

    public GameObject childFish;
    private Animator childAnimator;

    void Start()
    {
        fishHealthState = FishHealthState.Healthy;
        states = new List<FishState>();
        childAnimator = childFish.GetComponent<Animator>();
    }

    public void ChangeFishHealthState(FishHealthState newHealthState)
    {
        fishHealthState = newHealthState;
    }

    void FixedUpdate()
    {
        if (!Rewinder.instance.rewinding && Rewinder.instance.currentTimeFrame <= Rewinder.instance.maxTimeFrame)
        {
            MoveFish();
        }
    }

    void MoveFish()
    {
        transform.Translate(transform.forward * speed);
    }

    public override void Record()
    {
        childAnimator.enabled = true;
        if (Rewinder.instance.currentTimeFrame < states.Count)
        {
            states.RemoveRange(Rewinder.instance.currentTimeFrame, states.Count - Rewinder.instance.currentTimeFrame);
        }
        states.Add(new FishState(transform.position, transform.rotation, childFish.transform.localRotation, fishHealthState));
    }

    public override void RewindAt(int timeFrame)
    {
        childAnimator.enabled = false;
        FishState state = states[timeFrame];
        transform.position = state.position;
        transform.rotation = state.rotation;
        childFish.transform.rotation = state.childRotation;
        fishHealthState = state.healthState;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<OilSpill>())
        {
            ChangeFishHealthState(FishHealthState.Toxic);
            print("Fish became toxic");
        } else if (other.GetComponentInParent<Penguin>())
        {
            Penguin otherPenguin = other.GetComponentInParent<Penguin>();
            if (fishHealthState == FishHealthState.Healthy)
            {
                otherPenguin.ChangePenguinHealthState(Penguin.PenguinHealthState.HealthyJumping);
                print("Penguin became healthy");

            } else if (fishHealthState == FishHealthState.Toxic)
            {
                otherPenguin.ChangePenguinHealthState(Penguin.PenguinHealthState.PoisonedJumping);
                print("Penguin became toxic. NOOT NOOT");
            }

        }
    }
}

public class FishState
{
    public Vector3 position;
    public Quaternion rotation;
    public Quaternion childRotation;
    public Fish.FishHealthState healthState;

    public FishState(Vector3 position, Quaternion rotation, Quaternion childRotation, Fish.FishHealthState healthState)
    {
        this.position = position;
        this.rotation = rotation;
        this.childRotation = childRotation;
        this.healthState = healthState;
    }
}
