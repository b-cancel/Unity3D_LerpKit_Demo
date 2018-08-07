using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using lerpKit;
using UnityEngine.UI;
using System;

public class managerOfPoints : MonoBehaviour {

    //-------------------------PUBLIC VARS

    public GameObject stopwatchGO;
    public GameObject distanceSlider;
    public GameObject distanceDisplay;
    public GameObject distanceInputField;
    public GameObject timeInputField;
    public GameObject timeUnitDropDown;
    public GameObject updateLocationDropDown;

    public GameObject lerpValueSlider;
    public GameObject moveTowardsInputField;

    public GameObject label1;
    public GameObject label2;
    public GameObject label3;
    public GameObject label4;
    public GameObject label5;
    public GameObject label6;
    public GameObject label7;

    //LERP (t) slider
    public GameObject lerpPoint;
    public GameObject lerpWithDeltaTimePoint;
    public GameObject lerpWithFixedDeltaTimePoint;

    //MOVETOWARDS (maxDistanceDelta) slider
    public GameObject moveTowardsPoint;
    public GameObject moveTowardsWithDeltaTimePoint;
    public GameObject moveTowardsWithFixedDeltaTimePoint;

    //A TON OF OPTIONS
    public GameObject lerpHelperPoint;

    //-------------------------PRIVATE VARS

    float screenHeight;
    float screenWidth;
    Vector3 screeenTopLeftCorner;

    float startX;
    public float distanceToTravel;

    GameObject[] points;
    GameObject[] labels;
    Vector3[] prevPosition;

    //-------------------------CHANGEABLE VARS

    //lerp
    public float t; 
    //movetowards
    public float maxDistanceDelta;
    //lerpkit
    public updateLocation UL;
    public unitOfTime UOT;
    public float guideDistance;
    public float guideTime;

    // Use this for initialization
    void Awake () {

        distanceToTravel = 7.5f;
        distanceSlider.GetComponent<Slider>().value = distanceToTravel;
        distanceDisplay.GetComponent<Text>().text = distanceToTravel.ToString();

        t = .25f;
        lerpValueSlider.GetComponent<Slider>().value = t;
        maxDistanceDelta = 1f;
        moveTowardsInputField.GetComponent<InputField>().text = maxDistanceDelta.ToString();

        UL = updateLocation.fixedUpdate;
        UOT = unitOfTime.seconds;
        guideDistance = 10;
        distanceInputField.GetComponent<InputField>().text = guideDistance.ToString();
        guideTime = 5;
        timeInputField.GetComponent<InputField>().text = guideTime.ToString();

        points = new []
        {
            lerpPoint,
            lerpWithDeltaTimePoint,
            lerpWithFixedDeltaTimePoint,
            moveTowardsPoint,
            moveTowardsWithDeltaTimePoint,
            moveTowardsWithFixedDeltaTimePoint,
            lerpHelperPoint
        };

        labels = new[]
        {
            label1,
            label2,
            label3,
            label4,
            label5,
            label6,
            label7
        };

        prevPosition = new Vector3[7];

        prepPrivateVars();
        positionPointsOnStart();
    }
	
	// Update is called once per frame
	void Update () {
        if (stopwatchGO.GetComponent<stopwatch>().isPlaying)
        {
            float endX = startX + distanceToTravel;

            lerpPoint.transform.position = new Vector3(
            Mathf.Lerp(lerpPoint.transform.position.x, endX, t),
            lerpPoint.transform.position.y,
            0
            );

            lerpWithDeltaTimePoint.transform.position = new Vector3(
            Mathf.Lerp(lerpWithDeltaTimePoint.transform.position.x, endX, t * Time.deltaTime),
            lerpWithDeltaTimePoint.transform.position.y,
            0
            );

            moveTowardsPoint.transform.position = Vector3.MoveTowards(
                moveTowardsPoint.transform.position,
                new Vector3(
                    endX,
                    moveTowardsPoint.transform.position.y,
                    0
                    ),
                maxDistanceDelta
            );

            moveTowardsWithDeltaTimePoint.transform.position = Vector3.MoveTowards(
                moveTowardsWithDeltaTimePoint.transform.position,
                new Vector3(
                    endX,
                    moveTowardsWithDeltaTimePoint.transform.position.y,
                    0
                    ),
                maxDistanceDelta * Time.deltaTime
            );

            if(UL == updateLocation.update)
            {
                //NOTE: you can optimzie this step by calculating this only when you need it and running ONLY the lerp every frame
                float lerpVelocityDistancePerFrame = lerpHelper.calcLerpVelocity(guideDistance, guideTime, UOT, UL);
                //NOTE: this must happen every frame
                lerpHelperPoint.transform.position = new Vector3(
                    Mathf.Lerp(lerpHelperPoint.transform.position.x, endX, lerpHelper.calcLerpValue(lerpHelperPoint.transform.position.x, endX, lerpVelocityDistancePerFrame)),
                    lerpHelperPoint.transform.position.y,
                    0
                );
            }

            updatePointData();
        }

    }

    void FixedUpdate()
    {
        if (stopwatchGO.GetComponent<stopwatch>().isPlaying)
        {
            float endX = startX + distanceToTravel;

            lerpWithFixedDeltaTimePoint.transform.position = new Vector3(
            Mathf.Lerp(lerpWithFixedDeltaTimePoint.transform.position.x, endX, t * Time.fixedDeltaTime),
            lerpWithFixedDeltaTimePoint.transform.position.y,
            0
            );

            moveTowardsWithFixedDeltaTimePoint.transform.position = Vector3.MoveTowards(
                moveTowardsWithFixedDeltaTimePoint.transform.position,
                new Vector3(
                    endX,
                    moveTowardsWithFixedDeltaTimePoint.transform.position.y,
                    0
                    ),
                maxDistanceDelta * Time.fixedDeltaTime
            );

            if (UL == updateLocation.fixedUpdate)
            {
                //NOTE: you can optimzie this step by calculating this only when you need it and running ONLY the lerp every frame
                float lerpVelocityDistancePerFrame = lerpHelper.calcLerpVelocity(guideDistance, guideTime, UOT, UL);
                //NOTE: this must happen every frame
                lerpHelperPoint.transform.position = new Vector3(
                    Mathf.Lerp(lerpHelperPoint.transform.position.x, endX, lerpHelper.calcLerpValue(lerpHelperPoint.transform.position.x, endX, lerpVelocityDistancePerFrame)),
                    lerpHelperPoint.transform.position.y,
                    0
                );
            }
        }
    }

    void prepPrivateVars()
    {
        // Screens coordinate corner location
        var upperLeftScreen = new Vector3(0, Screen.height, 0);
        var upperRightScreen = new Vector3(Screen.width, Screen.height, 0);
        var lowerLeftScreen = new Vector3(0, 0, 0);
        var lowerRightScreen = new Vector3(Screen.width, 0, 0);

        //Corner locations in world coordinates
        var upperLeft = Camera.main.ScreenToWorldPoint(upperLeftScreen);
        var upperRight = Camera.main.ScreenToWorldPoint(upperRightScreen);
        var lowerLeft = Camera.main.ScreenToWorldPoint(lowerLeftScreen);
        var lowerRight = Camera.main.ScreenToWorldPoint(lowerRightScreen);

        screeenTopLeftCorner = new Vector3(upperLeft.x, upperLeft.y, 0);
        screenHeight = Mathf.Abs(upperLeft.y - lowerLeft.y);
        screenWidth = Mathf.Abs(upperLeft.x - upperRight.x);

        startX = upperLeft.x;

        distanceSlider.GetComponent<Slider>().maxValue = screenWidth;
    }

    public void reset()
    {
        positionPointsOnStart();
        foreach (GameObject point in points)
            point.GetComponent<SpriteRenderer>().color = Color.white;
    }

    void positionPointsOnStart()
    {
        float shiftValue = screenHeight / 8f;

        //NOTE: all points start at the same x value but must be spread out accorind to their y
        lerpPoint.transform.position = new Vector3(startX, screeenTopLeftCorner.y - (shiftValue * 1), 0);
        lerpWithDeltaTimePoint.transform.position = new Vector3(startX, screeenTopLeftCorner.y - (shiftValue * 2), 0);
        lerpWithFixedDeltaTimePoint.transform.position = new Vector3(startX, screeenTopLeftCorner.y - (shiftValue * 3), 0);

        moveTowardsPoint.transform.position = new Vector3(startX, screeenTopLeftCorner.y - (shiftValue * 4), 0);
        moveTowardsWithDeltaTimePoint.transform.position = new Vector3(startX, screeenTopLeftCorner.y - (shiftValue * 5), 0);
        moveTowardsWithFixedDeltaTimePoint.transform.position = new Vector3(startX, screeenTopLeftCorner.y - (shiftValue * 6), 0);

        lerpHelperPoint.transform.position = new Vector3(startX, screeenTopLeftCorner.y - (shiftValue * 7), 0);

        foreach (var label in labels)
            label.GetComponent<Text>().text = "";
    }

    void updatePointData()
    {
        for(int i = 0; i < points.Length; i++)
        {
            if (Mathf.Approximately(points[i].transform.position.x, (startX + distanceToTravel)))
            {
                if(points[i].GetComponent<SpriteRenderer>().color == Color.white)
                {
                    points[i].GetComponent<SpriteRenderer>().color = Color.red;
                    int seconds = (int)stopwatchGO.GetComponent<stopwatch>().timePassed;
                    int milliseconds = (int)((stopwatchGO.GetComponent<stopwatch>().timePassed - seconds) * 1000);
                    labels[i].GetComponent<Text>().text = "Finished in " + seconds + ":" + stopwatchGO.GetComponent<stopwatch>().atleastOfSizeN(milliseconds.ToString(),3) + " seconds";   
                }
            }
            else
            {
                if (points[i].GetComponent<SpriteRenderer>().color == Color.red)
                    points[i].GetComponent<SpriteRenderer>().color = Color.white;

                //cover edge case for first frame
                if (prevPosition[i] == null)
                    prevPosition[i] = points[i].transform.position;

                float velocity = Vector3.Distance(points[i].transform.position, prevPosition[i]) / Time.deltaTime;
                int seconds = (int)velocity;
                int milliseconds = (int)((velocity - seconds) * 1000);

                prevPosition[i] = points[i].transform.position;

                labels[i].GetComponent<Text>().text = "Velocity : " + seconds + "." + stopwatchGO.GetComponent<stopwatch>().atleastOfSizeN(milliseconds.ToString(), 3);

                labels[i].transform.position = new Vector3(points[i].transform.position.x - 3, points[i].transform.position.y, 0);
            }
        }
            
    }

    //-------------------------UI FUNCTIONS-------------------------

    public void distanceValueChanged()
    {
        distanceToTravel = distanceSlider.GetComponent<Slider>().value;
        distanceDisplay.GetComponent<Text>().text = distanceToTravel.ToString();
    }

    public void lerpValueChanged()
    {
        t = lerpValueSlider.GetComponent<Slider>().value;
    }

    public void moveTowardsValueChanged()
    {
        String result = moveTowardsInputField.GetComponent<InputField>().text;
        float res;
        if (float.TryParse(result, out res))
            maxDistanceDelta = float.Parse(result);
        else
            maxDistanceDelta = 0;
    }

    public void updateLocationChanged()
    {
        if (updateLocationDropDown.GetComponent<Dropdown>().value == 0)
            UL = updateLocation.fixedUpdate;
        else
            UL = updateLocation.update;
    }

    public void unitOfTimeChanged()
    {
        if (timeUnitDropDown.GetComponent<Dropdown>().value == 0)
            UOT = unitOfTime.seconds;
        else
            UOT = unitOfTime.frames;
    }

    public void guideDistChanged()
    {
        String result = distanceInputField.GetComponent<InputField>().text;
        float res;
        if (float.TryParse(result, out res))
            guideDistance = float.Parse(result);
        else
            guideDistance = 0;
    }

    public void guideTimeChanged()
    {
        String result = timeInputField.GetComponent<InputField>().text;
        float res;
        if (float.TryParse(result, out res))
            guideTime = float.Parse(result);
        else
            guideTime = 0;
    }
}
