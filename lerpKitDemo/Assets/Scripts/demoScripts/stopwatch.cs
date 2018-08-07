using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class stopwatch : MonoBehaviour {

    public GameObject pointManagerGO;

    public GameObject PlayStopBtn;
    public GameObject ResetBtn;
    public GameObject StopwatchDisplay;

    public bool isPlaying;
    public float timePassed;

    public int secondsRaw;
    public int minutes;
    public int seconds;
    public int milliseconds;
    public string mins;
    public string secs;
    public string milli;

	// Use this for initialization
	void Awake () {
        isPlaying = false;
        timePassed = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (isPlaying)
        {
            timePassed += Time.deltaTime;
            displayTimeOnTimer();
        }
	}

    void displayTimeOnTimer()
    {
        secondsRaw = ((int)timePassed);
        minutes = secondsRaw / 60;
        seconds = secondsRaw % 60;
        milliseconds = (int)((timePassed - secondsRaw) * 1000);

        mins = atleastOfSizeN(minutes.ToString(), 2);
        secs = atleastOfSizeN(seconds.ToString(), 2);
        milli = atleastOfSizeN(milliseconds.ToString(), 3);

        StopwatchDisplay.GetComponent<Text>().text = mins + " : " + secs + " : " + milli;
    }

    public string atleastOfSizeN(string str, int n)
    {
        string result = str;
        for (int i = (n - str.Length); i > 0; i--)
            result = "0" + str;
        return result;
    }

    public void startStop()
    {
        isPlaying = !isPlaying;
        if (isPlaying)
            PlayStopBtn.transform.GetChild(0).GetComponent<Text>().text = "Pause";
        else
            PlayStopBtn.transform.GetChild(0).GetComponent<Text>().text = "Play";
    }

    public void reset()
    {
        pointManagerGO.GetComponent<managerOfPoints>().reset();
        StopwatchDisplay.GetComponent<Text>().text = "00 : 00 : 000";
        timePassed = 0;
    }
    
    public void resetAndStop()
    {
        if (isPlaying) startStop();
        reset();
    }
}
