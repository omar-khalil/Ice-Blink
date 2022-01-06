using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : Rewindable
{
    public float speed;
    private float initialSpeed;

    private List<PenguinState> states;

    //Penguin randomized movement
    public float timeBetweenChange;
    private Vector3 displacement;
    private float timeSinceLastChange;

    public GameObject penguinChild;
    private Animator childAnimator;

    public Transform jumpPoint;
    public Transform homePoint;

    //Win
    public GameObject winPanel;
    public AudioClip winSound;

    public enum PenguinHealthState
    {
        Healthy,
        HealthyJumping,
        HealthyReturning,
        Jumping,
        Poisoned,
        PoisonedJumping,
        PoisonedReturning,
        Hungry,
        Dead
    }
    public PenguinHealthState currentHealthState;

    void Start()
    {
        //currentHealthState = PenguinHealthState.Hungry;
        states = new List<PenguinState>();
        timeSinceLastChange = timeBetweenChange;
        childAnimator = penguinChild.GetComponent<Animator>();
        initialSpeed = speed;
    }

    public void ChangePenguinHealthState(PenguinHealthState newHealthState)
    {
        currentHealthState = newHealthState;
    }

    public void Return()
    {
        if (currentHealthState == PenguinHealthState.HealthyJumping)
        {
            ChangePenguinHealthState(PenguinHealthState.HealthyReturning);
        }
        else if (currentHealthState == PenguinHealthState.PoisonedJumping)
        {
            ChangePenguinHealthState(PenguinHealthState.PoisonedReturning);
        }
    }

    void FixedUpdate()
    {
        if (!Rewinder.instance.rewinding && Rewinder.instance.currentTimeFrame <= Rewinder.instance.maxTimeFrame)
        {
            MovePenguin();
        }
    }

    void MovePenguin()
    {
        if (currentHealthState == PenguinHealthState.Healthy)
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
            childAnimator.SetBool("Walking", displacement.magnitude > 0);
        }
        else if (currentHealthState == PenguinHealthState.Hungry)
        {
            speed = initialSpeed;
            displacement = (jumpPoint.position - transform.position).normalized * speed;
            transform.forward = displacement;
            transform.position += displacement;
            childAnimator.SetBool("Walking", displacement.magnitude > 0);
        }
        else if (currentHealthState == PenguinHealthState.HealthyReturning ||
          currentHealthState == PenguinHealthState.PoisonedReturning)
        {
            speed = initialSpeed;
            displacement = (homePoint.position - transform.position).normalized * speed;
            transform.forward = displacement;
            transform.position += displacement;
            childAnimator.SetBool("Walking", displacement.magnitude > 0);
        }

    }

    public override void Record()
    {
        childAnimator.enabled = true;
        if (Rewinder.instance.currentTimeFrame < states.Count)
        {
            states.RemoveRange(Rewinder.instance.currentTimeFrame, states.Count - Rewinder.instance.currentTimeFrame);
        }
        states.Add(new PenguinState(transform.position, transform.rotation, penguinChild.transform.position, penguinChild.transform.rotation, currentHealthState));
    }

    void Win()
    {
        winPanel.SetActive(true);
        SoundManager.instance.PlaySound(winSound, false);
    }

    public override void RewindAt(int timeFrame)
    {
        childAnimator.enabled = false;
        PenguinState state = states[timeFrame];
        transform.position = state.position;
        transform.rotation = state.rotation;
        penguinChild.transform.position = state.childPosition;
        penguinChild.transform.rotation = state.childRotation;
        currentHealthState = state.healthState;
    }

    void OnTriggerEnter(Collider other)
    {
        print("Collission on penguin detected with " + other.name);
        if (other.CompareTag("JumpPoint") && currentHealthState == PenguinHealthState.Hungry)
        {
            ChangePenguinHealthState(PenguinHealthState.Jumping);
            childAnimator.SetTrigger("Jump");
        }
        else if (other.CompareTag("HomePoint"))
        {
            if (currentHealthState == PenguinHealthState.PoisonedReturning)
            {
                currentHealthState = PenguinHealthState.Dead;
                childAnimator.SetBool("Walking", false);
                transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.0045f, transform.position.z);
            }
            else if (currentHealthState == PenguinHealthState.HealthyReturning)
            {
                currentHealthState = PenguinHealthState.Healthy;
                Win();
            }
        }
    }
}

public class PenguinState
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 childPosition;
    public Quaternion childRotation;
    public Penguin.PenguinHealthState healthState;

    public PenguinState(Vector3 position, Quaternion rotation, Vector3 childPosition, Quaternion childRotation, Penguin.PenguinHealthState healthState)
    {
        this.position = position;
        this.rotation = rotation;
        this.childPosition = childPosition;
        this.childRotation = childRotation;
        this.healthState = healthState;
    }
}
