using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Tooltip("Max game time in seconds")]
    public float maxTime;

    public Text scoreText;
    public Text timeText;

    private float currTime = 0f;

    void Update()
    {
        SetTimeText();
    }

    private void SetTimeText()
    {
        currTime += Time.deltaTime;
        if (currTime < maxTime)
        {
            float remainingTime = maxTime - currTime;
            int minutes = (int)Math.Floor(remainingTime / 60);
            int seconds = (int)(remainingTime % 60);
            timeText.text = minutes.ToString("00") + ':' + seconds.ToString("00");
        }
        else
        {
            timeText.text = "00:00";
        }
    }
}
