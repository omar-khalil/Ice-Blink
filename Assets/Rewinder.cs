using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rewinder : MonoBehaviour {

    public static Rewinder instance;

    public AudioClip rewindTick;
    public AudioClip click;

    //public bool recording;
    public bool rewinding = false;
    public int currentTimeFrame;
    public int maxTimeFrame; //should be 500 for 10 seconds
    public Rewindable[] rewindables;

    //UI
    public Slider timeSlider;
    public Text currentTimeFrameText;
    bool ignoreSliderValueChange;

    void Awake()
    {
        instance = this;
    }

	// Use this for initialization
	void Start () {
        rewindables = FindObjectsOfType<Rewindable>();
        timeSlider.maxValue = maxTimeFrame;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleRewind();
        }
        ignoreSliderValueChange = true;
        timeSlider.value = currentTimeFrame;
        ignoreSliderValueChange = false;
	}

    public void ToggleRewind()
    {
        SoundManager.instance.PlaySound(click, true);
        rewinding = true;
        Time.timeScale = 1f;
    }

    void FixedUpdate()
    {
        currentTimeFrameText.text = currentTimeFrame + "";
        if (rewinding)
        {
            if (currentTimeFrame > 0)
            {
                currentTimeFrame--;
                foreach(Rewindable r in rewindables)
                {
                    r.RewindAt(currentTimeFrame);
                }
            }
        } else
        {
            if (currentTimeFrame <= maxTimeFrame)
            {
                foreach(Rewindable r in rewindables)
                {
                    r.Record();
                }
                currentTimeFrame++;
            }
        }
    }

    //UI controls

    public void Pause()
    {
        SoundManager.instance.PlaySound(click, true);
        Time.timeScale = 0f;
        //Time.fixedDeltaTime = 0f;//0.02f * Time.timeScale;
    }

    public void Play()
    {
        SoundManager.instance.PlaySound(click, true);
        Time.timeScale = 1f;
        rewinding = false;
        //Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    public void GoToTimeFrame(float timeFrame)
    {
        if (!ignoreSliderValueChange)
        {
            SoundManager.instance.PlaySound(rewindTick, true); 
        }
        if (!ignoreSliderValueChange)
        {
            Pause();
            foreach (Rewindable r in rewindables)
            {
                r.RewindAt((int)timeFrame);
            }
            currentTimeFrame = (int)timeFrame;
            currentTimeFrameText.text = currentTimeFrame + "";
        }
    }
}
